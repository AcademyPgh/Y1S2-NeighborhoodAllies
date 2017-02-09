namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ImpactMap.Models.ImpactMapDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ImpactMap.Models.ImpactMapDbContext context)
        {
            //context.categories.AddOrUpdate(
            //    new Models.Category { ID = 1, name = "Economic Development", isBase = true },
            //    new Models.Category { ID = 1, name = "Food Insecurity", isBase = true },
            //    new Models.Category { ID = 1, name = "Job Creation", isBase = true },
            //    new Models.Category { ID = 1, name = "Facilities Improvement", isBase = true }
            //    );
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

        
    }
}
