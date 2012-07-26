using System;
using System.Linq;
using System.Security.Cryptography;

namespace Marina.Store.Web.Policies
{
    /// <summary>
    /// Правила создания хэша пароля и его проверки
    /// </summary>
    public class PasswordHashPolicy
    {
        public const int SALT_LENGTH = 50;
        public const int HASH_LENGTH = 50;
        public const int ITERATIONS = 1000;
        public const char DELIMITER = ':';

        /// <summary>
        /// Сформировать хэш пароля
        /// </summary>
        public virtual string Apply(string password)
        {
            // генерируем случайную соль для каждого пароля (даже одинаковые пароли будут иметь разный хэш)
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[SALT_LENGTH];
            csprng.GetBytes(salt);

            // генерируем хэш пароля, используя соль и key stretching (реализацию PDKDF2 от Microsoft)
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = ITERATIONS };
            var hash = pbkdf2.GetBytes(HASH_LENGTH);

            // соль сохраняем вместе с хэшем (соль -- не секрет)
            return Convert.ToBase64String(salt) + DELIMITER + Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Проверить пароль по хэшу
        /// </summary>
        public virtual bool Check(string password, string hashedPassword)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentNullException("hashedPassword");
            }

            var split = hashedPassword.Split(DELIMITER);

            if (split.Length != 2)
            {
                throw new InvalidOperationException("Невозможно проверить пароль по хэшу: несовместимый формат хэша");
            }

            var salt = Convert.FromBase64String(split[0]);
            var hash = Convert.FromBase64String(split[1]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt) { IterationCount = ITERATIONS };

            return pbkdf2.GetBytes(hash.Length).SequenceEqual(hash);
        }
    }
}