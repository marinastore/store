using System;
using System.Linq;
using System.Text.RegularExpressions;
using Marina.Store.Web.DataAccess;
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

        public CommandResult Execute(string userEmail)
        {
            // создаем заявку, связываем с корзиной, отправляем по емайлу

            var emailRegexp = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            if (!emailRegexp.IsMatch(userEmail))
            {
                return Fail("userEmail", "Невалидный емайл: " + userEmail);
            }


            var request = new RegistrationRequest();
            request.Email = userEmail;
            request.Id = Guid.NewGuid();

            if (_db.Users.Any(u => u.Email == userEmail))
            {
                return Fail("userEmail", "Пользователь с таким емайлом уже существует");
            }

            var cartResult = _getCartCmd.Execute();

            if (cartResult.HasErrors)
            {
                return Fail(cartResult.Errors);
            }
            if (cartResult.Model.Items.Any())
            {
                request.Cart = cartResult.Model;
            }
            _db.RegistrationRequests.Add(request);

            _mailService.SendUserConfirmatiom(userEmail, request.Id.ToString());

            return Success();
        }
    }
}