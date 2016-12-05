namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Whatever : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        entityID = c.Int(nullable: false),
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
                        Project_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.categoryID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_ID)
                .Index(t => t.categoryID)
                .Index(t => t.Project_ID);
            
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
                        entityType = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
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
            
            CreateTable(
                "dbo.Investments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        amount = c.String(),
                        entityFrom = c.Int(nullable: false),
                        entityTo = c.Int(nullable: false),
                        projectFrom = c.Int(nullable: false),
                        projectTo = c.Int(nullable: false),
                        date = c.DateTime(nullable: false),
                        isInKind = c.Boolean(nullable: false),
                        volunteerHours = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProjectMetricResults",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        projectID = c.Int(nullable: false),
                        metricID = c.Int(nullable: false),
                        metricValue = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        entityID = c.String(),
                        description = c.String(),
                        investmentIn = c.String(),
                        investmentOut = c.String(),
                        isPassThrough = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        projectID = c.Int(nullable: false),
                        completed = c.Boolean(nullable: false),
                        reportText = c.String(),
                        dueDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Metrics", "Project_ID", "dbo.Projects");
            DropForeignKey("dbo.Categories", "Investment_ID", "dbo.Investments");
            DropForeignKey("dbo.Users", "Entity_ID", "dbo.Entities");
            DropForeignKey("dbo.Metrics", "categoryID", "dbo.Categories");
            DropIndex("dbo.Users", new[] { "Entity_ID" });
            DropIndex("dbo.Metrics", new[] { "Project_ID" });
            DropIndex("dbo.Metrics", new[] { "categoryID" });
            DropIndex("dbo.Categories", new[] { "Investment_ID" });
            DropTable("dbo.Reports");
            DropTable("dbo.Projects");
            DropTable("dbo.ProjectMetricResults");
            DropTable("dbo.Investments");
            DropTable("dbo.Users");
            DropTable("dbo.Entities");
            DropTable("dbo.Metrics");
            DropTable("dbo.Categories");
        }
    }
}
