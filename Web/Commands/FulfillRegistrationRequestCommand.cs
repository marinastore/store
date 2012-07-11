using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Marina.Store.Web.DataAccess;
using Marina.Store.Web.Models;

namespace Marina.Store.Web.Commands
{
    public class FulfillRegistrationRequestCommand : Command
    {
        private readonly StoreDbContext _db;
        private readonly GetShoppingCartCommand _getCartCmd;

        public FulfillRegistrationRequestCommand(StoreDbContext db, GetShoppingCartCommand getCartCmd)
        {
            _db = db;
            _getCartCmd = getCartCmd;
        }
        
        public CommandResult Execute(Guid registrationRequestId, string password)
        {
            var request = _db.RegistrationRequests.First(r => r.Id == registrationRequestId);
            if (request == null)
            {
                return Fail("request", "Не существует заявки с id " + registrationRequestId);
            }

            string passwordHash;
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                // TODO: использовать соль!
                passwordHash = Encoding.UTF8.GetString(sha1.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }

            var user = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash
            };

            _db.Users.Add(user);

            return Success();
        }
    }
}