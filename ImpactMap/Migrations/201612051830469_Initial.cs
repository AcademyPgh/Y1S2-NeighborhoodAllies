namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        Investment_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Investments", t => t.Investment_ID)
                .Index(t => t.Investment_ID);
            
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        address1 = c.String(),
                        address2 = c.String(),
                        city = c.String(),
                        state = c.String(),
                        zip = c.String(),
                        lat = c.String(),
                        lng = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Investments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        amount = c.String(),
                        entityFrom_ID = c.Int(),
                        entityTo_ID = c.Int(),
                        date = c.DateTime(nullable: false),
                        isInKind = c.Boolean(nullable: false),
                        volunteerHours = c.String(),
                        projectFrom_ID = c.Int(),
                        projectTo_ID = c.Int(),
                        entityFrom_ID1 = c.Int(),
                        entityTo_ID1 = c.Int(),
                        projectFrom_ID1 = c.Int(),
                        projectTo_ID1 = c.Int(),
                        Entity_ID = c.Int(),
                        Entity_ID1 = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Entities", t => t.entityFrom_ID1)
                .ForeignKey("dbo.Entities", t => t.entityTo_ID1)
                .ForeignKey("dbo.Projects", t => t.projectFrom_ID1)
                .ForeignKey("dbo.Projects", t => t.projectTo_ID1)
                .ForeignKey("dbo.Entities", t => t.Entity_ID)
                .ForeignKey("dbo.Entities", t => t.Entity_ID1)
                .Index(t => t.entityFrom_ID1)
                .Index(t => t.entityTo_ID1)
                .Index(t => t.projectFrom_ID1)
                .Index(t => t.projectTo_ID1)
                .Index(t => t.Entity_ID)
                .Index(t => t.Entity_ID1);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        entity_ID = c.Int(),
                        investmentIn_ID = c.Int(),
                        investmentOut_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Entities", t => t.entity_ID)
                .ForeignKey("dbo.Investments", t => t.investmentIn_ID)
                .ForeignKey("dbo.Investments", t => t.investmentOut_ID)
                .Index(t => t.entity_ID)
                .Index(t => t.investmentIn_ID)
                .Index(t => t.investmentOut_ID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        name = c.String(),
                        completed = c.Boolean(nullable: false),
                        reportText = c.String(),
                        dueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ID)
                .Index(t => t.ID);
            
            CreateTable(
                "dbo.Metrics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        categoryID = c.Int(nullable: false),
                        name = c.String(),
                        description = c.String(),
                        Report_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Reports", t => t.Report_ID)
                .Index(t => t.Report_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userModelGuid = c.Guid(nullable: false),
                        userModelName = c.String(),
                        Entity_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Entities", t => t.Entity_ID)
                .Index(t => t.Entity_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "Entity_ID", "dbo.Entities");
            DropForeignKey("dbo.Investments", "Entity_ID1", "dbo.Entities");
            DropForeignKey("dbo.Investments", "Entity_ID", "dbo.Entities");
            DropForeignKey("dbo.Investments", "projectTo_ID1", "dbo.Projects");
            DropForeignKey("dbo.Investments", "projectFrom_ID1", "dbo.Projects");
            DropForeignKey("dbo.Reports", "ID", "dbo.Projects");
            DropForeignKey("dbo.Metrics", "Report_ID", "dbo.Reports");
            DropForeignKey("dbo.Projects", "investmentOut_ID", "dbo.Investments");
            DropForeignKey("dbo.Projects", "investmentIn_ID", "dbo.Investments");
            DropForeignKey("dbo.Projects", "entity_ID", "dbo.Entities");
            DropForeignKey("dbo.Investments", "entityTo_ID1", "dbo.Entities");
            DropForeignKey("dbo.Investments", "entityFrom_ID1", "dbo.Entities");
            DropForeignKey("dbo.Categories", "Investment_ID", "dbo.Investments");
            DropIndex("dbo.Users", new[] { "Entity_ID" });
            DropIndex("dbo.Metrics", new[] { "Report_ID" });
            DropIndex("dbo.Reports", new[] { "ID" });
            DropIndex("dbo.Projects", new[] { "investmentOut_ID" });
            DropIndex("dbo.Projects", new[] { "investmentIn_ID" });
            DropIndex("dbo.Projects", new[] { "entity_ID" });
            DropIndex("dbo.Investments", new[] { "Entity_ID1" });
            DropIndex("dbo.Investments", new[] { "Entity_ID" });
            DropIndex("dbo.Investments", new[] { "projectTo_ID1" });
            DropIndex("dbo.Investments", new[] { "projectFrom_ID1" });
            DropIndex("dbo.Investments", new[] { "entityTo_ID1" });
            DropIndex("dbo.Investments", new[] { "entityFrom_ID1" });
            DropIndex("dbo.Categories", new[] { "Investment_ID" });
            DropTable("dbo.Users");
            DropTable("dbo.Metrics");
            DropTable("dbo.Reports");
            DropTable("dbo.Projects");
            DropTable("dbo.Investments");
            DropTable("dbo.Entities");
            DropTable("dbo.Categories");
        }
    }
}
