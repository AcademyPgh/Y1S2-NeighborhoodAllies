namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        isBase = c.Boolean(nullable: false),
                        entityID = c.Int(),
                        baseID = c.Int(),
                        Investment_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Investments", t => t.Investment_ID)
                .Index(t => t.Investment_ID);
            
            CreateTable(
                "dbo.Metrics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        categoryID = c.Int(nullable: false),
                        name = c.String(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.categoryID, cascadeDelete: true)
                .Index(t => t.categoryID);
            
            CreateTable(
                "dbo.Entities",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        logoURL = c.String(),
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
                        amount = c.String(nullable: false),
                        entityFrom_ID = c.Int(),
                        entityTo_ID = c.Int(),
                        date = c.DateTime(nullable: false),
                        isInKind = c.Boolean(nullable: false),
                        volunteerHours = c.String(),
                        projectFrom_ID = c.Int(),
                        projectTo_ID = c.Int(),
                        description = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Entities", t => t.entityFrom_ID)
                .ForeignKey("dbo.Entities", t => t.entityTo_ID)
                .ForeignKey("dbo.Projects", t => t.projectFrom_ID)
                .ForeignKey("dbo.Projects", t => t.projectTo_ID)
                .Index(t => t.entityFrom_ID)
                .Index(t => t.entityTo_ID)
                .Index(t => t.projectFrom_ID)
                .Index(t => t.projectTo_ID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        entity_ID = c.Int(),
                        report_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Entities", t => t.entity_ID)
                .ForeignKey("dbo.Reports", t => t.report_ID)
                .Index(t => t.entity_ID)
                .Index(t => t.report_ID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        completed = c.Boolean(nullable: false),
                        reportText = c.String(),
                        dueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MetricResults",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        resultText = c.String(),
                        metric_ID = c.Int(),
                        report_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Metrics", t => t.metric_ID)
                .ForeignKey("dbo.Reports", t => t.report_ID)
                .Index(t => t.metric_ID)
                .Index(t => t.report_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userModelGuid = c.Guid(nullable: false),
                        userModelName = c.String(),
                        entity_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Entities", t => t.entity_ID)
                .Index(t => t.entity_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "entity_ID", "dbo.Entities");
            DropForeignKey("dbo.Investments", "projectTo_ID", "dbo.Projects");
            DropForeignKey("dbo.Investments", "projectFrom_ID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "report_ID", "dbo.Reports");
            DropForeignKey("dbo.MetricResults", "report_ID", "dbo.Reports");
            DropForeignKey("dbo.MetricResults", "metric_ID", "dbo.Metrics");
            DropForeignKey("dbo.Projects", "entity_ID", "dbo.Entities");
            DropForeignKey("dbo.Investments", "entityTo_ID", "dbo.Entities");
            DropForeignKey("dbo.Investments", "entityFrom_ID", "dbo.Entities");
            DropForeignKey("dbo.Categories", "Investment_ID", "dbo.Investments");
            DropForeignKey("dbo.Metrics", "categoryID", "dbo.Categories");
            DropIndex("dbo.Users", new[] { "entity_ID" });
            DropIndex("dbo.MetricResults", new[] { "report_ID" });
            DropIndex("dbo.MetricResults", new[] { "metric_ID" });
            DropIndex("dbo.Projects", new[] { "report_ID" });
            DropIndex("dbo.Projects", new[] { "entity_ID" });
            DropIndex("dbo.Investments", new[] { "projectTo_ID" });
            DropIndex("dbo.Investments", new[] { "projectFrom_ID" });
            DropIndex("dbo.Investments", new[] { "entityTo_ID" });
            DropIndex("dbo.Investments", new[] { "entityFrom_ID" });
            DropIndex("dbo.Metrics", new[] { "categoryID" });
            DropIndex("dbo.Categories", new[] { "Investment_ID" });
            DropTable("dbo.Users");
            DropTable("dbo.MetricResults");
            DropTable("dbo.Reports");
            DropTable("dbo.Projects");
            DropTable("dbo.Investments");
            DropTable("dbo.Entities");
            DropTable("dbo.Metrics");
            DropTable("dbo.Categories");
        }
    }
}
