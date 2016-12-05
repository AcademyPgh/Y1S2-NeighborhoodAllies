namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ImpactMap.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ImpactMap.Models.ImpactMapDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ImpactMap.Models.ImpactMapDbContext context)
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
            context.entities.AddOrUpdate(e => e.ID,
                new Entity { ID = 1,
                    name = "Bridget's Doggo Care",
                    description = "A good place to get your doggo washed",
                    address1 = "122 F Street",
                    address2 = "",
                    city = "Pittsburgh",
                    state = "PA",
                    zip = "",
                    lat = "",
                    lng = "",
                    //entityType = "recipient"
                },
                new Entity
                {
                    ID = 2,
                    name = "Amanda's Doggo Country",
                    description = "A good place to store your doggos while you take a night on the town",
                    address1 = "344 G Street",
                    address2 = "",
                    city = "Pittsburgh",
                    state = "PA",
                    zip = "",
                    lat = "",
                    lng = "",
                    //entityType = "recipient"
                },
                new Entity
                {
                    ID = 3,
                    name = "GENERAL DONATIONS, INC",
                    description = "WE DONATE MONEY FOR NON-NEFARIOUS PURPOSES",
                    address1 = "1921 9th Avenue",
                    address2 = "",
                    city = "New York",
                    state = "NY",
                    zip = "",
                    lat = "",
                    lng = "",
                    //entityType = "donor"
                },
                new Entity
                {
                    ID = 4,
                    name = "ACME Money Laundering, LLC",
                    description = "A good place to get your money washed",
                    address1 = "1555 A Street",
                    address2 = "",
                    city = "Detroit",
                    state = "MI",
                    zip = "",
                    lat = "",
                    lng = "",
                    //entityType = "recipient"
                });

            context.projects.AddOrUpdate(p => p.ID,
                new Project
                {
                    ID = 1,
                    name = "New Doggo Brushes",
                    entityID = "1",
                    description = "Making sure our doggos have nice new brushes for their hairs ;)",
                    investmentIn = null,
                    investmentOut = null,
                    //metrics = { ID = 1, ID = 3, ID = 7 },
                    isPassThrough = false,
                },
                new Project
                {
                    ID = 2,
                    name = "Freshen Up Hay",
                    entityID = "2",
                    description = "Getting fresh hay for our doggos to roll around in! ;)",
                    investmentIn = null,
                    investmentOut = null,
                    isPassThrough = false,
                });
        }
    }
}
