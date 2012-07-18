using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Marina.Store.Web.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Marina.Store.Tests
{
    /// <summary>
    /// Тестовый класс, подготавливающий базу до вызова остальных тестов
    /// </summary>
    [TestClass]
    public class InitTest
    {
        [AssemblyInitialize]
        public static void InitStore(TestContext ctx)
        {
            using (var db = new StoreDbContext())
            {
                if (db.Database.Exists())
                {
                    db.Database.Delete();
                }
                db.Database.Create();
            }
        }
    }
}