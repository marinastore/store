﻿using System.Data.Entity.Migrations;

namespace Marina.Store.Web.DataAccess.Initializers
{
    public class AutoMigrationsConfiguration : DbMigrationsConfiguration<StoreDbContext>
    {
        public AutoMigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
