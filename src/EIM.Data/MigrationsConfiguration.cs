namespace EIM.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class MigrationsConfiguration : DbMigrationsConfiguration<EIM.Data.EIMDbContext>
    {
        public MigrationsConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "EIM.Data.EIMDbContext";
        }

        protected override void Seed(EIM.Data.EIMDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

        public static void SetInitializer()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EIMDbContext, MigrationsConfiguration>());
        }
    }
}
