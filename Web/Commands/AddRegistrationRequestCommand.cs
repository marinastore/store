using System;
using System.Linq;
using System.Text.RegularExpressions;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Infrastructure.Commands;
using Marina.Store.Web.MailService;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.Commands
{
    public class AddRegistrationRequestCommand : Command
    {
        private readonly StoreDbContext _db;
        private readonly GetShoppingCartCommand _getCartCmd;
        private readonly IMailService _mailService;

        public AddRegistrationRequestCommand(StoreDbContext db, GetShoppingCartCommand getCartCmd, IMailService mailService)
        {
            _db = db;
            _getCartCmd = getCartCmd;
            _mailService = mailService;
        }

        public State InvalidEmail;
        public State EmailAlreadyRegistered;

        public Result Execute(string userEmail)
        {
            if (!IsValidEmail(userEmail))
            {
                return Fail(() => InvalidEmail, "Невалидный email: " + userEmail);
            }

            var request = new RegistrationRequest
            {
                Id = Guid.NewGuid(),
                Email = userEmail
            };

            if (IsEmailRegistered(userEmail))
            {
                return Fail(() => EmailAlreadyRegistered, "Пользователь с таким email уже существует");
            }

            // получаем корзину
            var cartResult = _getCartCmd.Execute(GetShoppingCartCommand.FetchMode.GetOrCreate);
            if (cartResult.HasErrors)
            {
                return cartResult.Outcome;
            }

            // если корзина не пуста, созраняем ссылку в заявке
            if (cartResult.Value.Items.Any())
            {
                request.Cart = cartResult.Value;
            }

            // добавляем заявку
            _db.RegistrationRequests.Add(request);

            // отправляем письмо
            _mailService.SendUserConfirmatiom(userEmail, request.Id.ToString());

            return Ok();
        }

        #region Private helpers

        private bool IsEmailRegistered(string userEmail)
        {
            return _db.Users.Any(u => u.Email == userEmail);
        }

        private static bool IsValidEmail(string userEmail)
        {
            return new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*").IsMatch(userEmail);
        }

        #endregion
    }
}