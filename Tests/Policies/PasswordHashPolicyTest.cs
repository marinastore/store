using Marina.Store.Web.Policies;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests.Policies
{
    [TestClass]
    public class PasswordHashPolicyTest
    {
        private const string PASSWORD = "qwerty";

        /// <summary>
        /// Должен создаваться хэш
        /// </summary>
        [TestMethod]
        public void Must_create_hash()
        {
            // Arrannge

            // Act

            var policy = new PasswordHashPolicy();
            var hash = policy.Apply(PASSWORD);

            // Assert

            Assert.IsNotNull(hash, "Хэш не создался");
            Assert.AreNotEqual(PASSWORD, hash, "Вместо хэша вернулся пароль");
            var split = hash.Split(PasswordHashPolicy.DELIMITER);
            Assert.AreEqual(2, split.Length, "С хэшем не сохранилась соль");
        }

        /// <summary>
        /// Хэш должен содержать соль, с которой он был сгенерирован
        /// </summary>
        [TestMethod]
        public void Must_store_salt_with_hash()
        {
            // Arrannge

            // Act

            var policy = new PasswordHashPolicy();
            var hash = policy.Apply(PASSWORD);

            // Assert

            var split = hash.Split(PasswordHashPolicy.DELIMITER);
            Assert.AreEqual(2, split.Length, "С хэшем не сохранилась соль");
            Assert.AreNotEqual(split[0], split[1], "Хэш совпадает с солью");
        }

        /// <summary>
        /// Для одинаковых паролей подлжы создаваться уникальные хэши
        /// </summary>
        [TestMethod]
        public void Must_produce_different_hashed_for_same_passwords()
        {
            // Arrange

            // Act

            var policy = new PasswordHashPolicy();
            var hash1 = policy.Apply(PASSWORD);
            var hash2 = policy.Apply(PASSWORD);

            // Assert

            Assert.AreNotEqual(hash1, hash2, "Хэши одинаковых паролей совпали: скорее всего, не используется соль");
        }

        /// <summary>
        /// Можно проверить пароль по сгенерированному хэшу
        /// </summary>
        [TestMethod]
        public void Must_verify_password_by_hash()
        {
            // Arrange

            var policy = new PasswordHashPolicy();
            var hash = policy.Apply(PASSWORD);

            // Act

            var result = policy.Check(PASSWORD, hash);

            // Assert

            Assert.IsTrue(result, "Не работает проверка по хэшу");
        }
    }
}