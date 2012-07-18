using System.Data.Entity.Migrations;

namespace Marina.Store.Web.DataAccess.Initializers
{
    public class AutoMigrationsConfig : DbMigrationsConfiguration<StoreDbContext>
    {
        public AutoMigrationsConfig()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}