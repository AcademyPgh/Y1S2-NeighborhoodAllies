namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixApplicationDb : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "Investment_ID", "dbo.Investments");
            DropForeignKey("dbo.Investments", "entityFrom_ID1", "dbo.Entities");
            DropForeignKey("dbo.Investments", "entityTo_ID1", "dbo.Entities");
            DropForeignKey("dbo.Projects", "entity_ID", "dbo.Entities");
            DropForeignKey("dbo.Projects", "investmentIn_ID", "dbo.Investments");
            DropForeignKey("dbo.Projects", "investmentOut_ID", "dbo.Investments");
            DropForeignKey("dbo.MetricResults", "metric_ID", "dbo.Metrics");
            DropForeignKey("dbo.MetricResults", "report_ID", "dbo.Reports");
            DropForeignKey("dbo.Reports", "ID", "dbo.Projects");
            DropForeignKey("dbo.Investments", "projectFrom_ID1", "dbo.Projects");
            DropForeignKey("dbo.Investments", "projectTo_ID1", "dbo.Projects");
            DropForeignKey("dbo.Investments", "Entity_ID", "dbo.Entities");
            DropForeignKey("dbo.Investments", "Entity_ID1", "dbo.Entities");
            DropForeignKey("dbo.Users", "Entity_ID", "dbo.Entities");
            DropIndex("dbo.Categories", new[] { "Investment_ID" });
            DropIndex("dbo.Investments", new[] { "entityFrom_ID1" });
            DropIndex("dbo.Investments", new[] { "entityTo_ID1" });
            DropIndex("dbo.Investments", new[] { "projectFrom_ID1" });
            DropIndex("dbo.Investments", new[] { "projectTo_ID1" });
            DropIndex("dbo.Investments", new[] { "Entity_ID" });
            DropIndex("dbo.Investments", new[] { "Entity_ID1" });
            DropIndex("dbo.Projects", new[] { "entity_ID" });
            DropIndex("dbo.Projects", new[] { "investmentIn_ID" });
            DropIndex("dbo.Projects", new[] { "investmentOut_ID" });
            DropIndex("dbo.Reports", new[] { "ID" });
            DropIndex("dbo.MetricResults", new[] { "metric_ID" });
            DropIndex("dbo.MetricResults", new[] { "report_ID" });
            DropIndex("dbo.Users", new[] { "Entity_ID" });
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            DropTable("dbo.Categories");
            DropTable("dbo.Entities");
            DropTable("dbo.Investments");
            DropTable("dbo.Projects");
            DropTable("dbo.Reports");
            DropTable("dbo.MetricResults");
            DropTable("dbo.Metrics");
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        userModelGuid = c.Guid(nullable: false),
                        userModelName = c.String(),
                        Entity_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Metrics",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        categoryID = c.Int(nullable: false),
                        name = c.String(),
                        description = c.String(),
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
                .PrimaryKey(t => t.ID);
            
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
                .PrimaryKey(t => t.ID);
            
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
                        description = c.String(),
                        entityFrom_ID1 = c.Int(),
                        entityTo_ID1 = c.Int(),
                        projectFrom_ID1 = c.Int(),
                        projectTo_ID1 = c.Int(),
                        Entity_ID = c.Int(),
                        Entity_ID1 = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
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
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        Investment_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            CreateIndex("dbo.Users", "Entity_ID");
            CreateIndex("dbo.MetricResults", "report_ID");
            CreateIndex("dbo.MetricResults", "metric_ID");
            CreateIndex("dbo.Reports", "ID");
            CreateIndex("dbo.Projects", "investmentOut_ID");
            CreateIndex("dbo.Projects", "investmentIn_ID");
            CreateIndex("dbo.Projects", "entity_ID");
            CreateIndex("dbo.Investments", "Entity_ID1");
            CreateIndex("dbo.Investments", "Entity_ID");
            CreateIndex("dbo.Investments", "projectTo_ID1");
            CreateIndex("dbo.Investments", "projectFrom_ID1");
            CreateIndex("dbo.Investments", "entityTo_ID1");
            CreateIndex("dbo.Investments", "entityFrom_ID1");
            CreateIndex("dbo.Categories", "Investment_ID");
            AddForeignKey("dbo.Users", "Entity_ID", "dbo.Entities", "ID");
            AddForeignKey("dbo.Investments", "Entity_ID1", "dbo.Entities", "ID");
            AddForeignKey("dbo.Investments", "Entity_ID", "dbo.Entities", "ID");
            AddForeignKey("dbo.Investments", "projectTo_ID1", "dbo.Projects", "ID");
            AddForeignKey("dbo.Investments", "projectFrom_ID1", "dbo.Projects", "ID");
            AddForeignKey("dbo.Reports", "ID", "dbo.Projects", "ID");
            AddForeignKey("dbo.MetricResults", "report_ID", "dbo.Reports", "ID");
            AddForeignKey("dbo.MetricResults", "metric_ID", "dbo.Metrics", "ID");
            AddForeignKey("dbo.Projects", "investmentOut_ID", "dbo.Investments", "ID");
            AddForeignKey("dbo.Projects", "investmentIn_ID", "dbo.Investments", "ID");
            AddForeignKey("dbo.Projects", "entity_ID", "dbo.Entities", "ID");
            AddForeignKey("dbo.Investments", "entityTo_ID1", "dbo.Entities", "ID");
            AddForeignKey("dbo.Investments", "entityFrom_ID1", "dbo.Entities", "ID");
            AddForeignKey("dbo.Categories", "Investment_ID", "dbo.Investments", "ID");
        }
    }
}
