namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Hello : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Investments", "projectFrom_ID", c => c.Int());
            AddColumn("dbo.Investments", "projectTo_ID", c => c.Int());
            CreateIndex("dbo.Investments", "projectFrom_ID");
            CreateIndex("dbo.Investments", "projectTo_ID");
            AddForeignKey("dbo.Investments", "projectFrom_ID", "dbo.Projects", "ID");
            AddForeignKey("dbo.Investments", "projectTo_ID", "dbo.Projects", "ID");
            DropColumn("dbo.Entities", "entityType");
            DropColumn("dbo.Investments", "projectFrom");
            DropColumn("dbo.Investments", "projectTo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Investments", "projectTo", c => c.Int(nullable: false));
            AddColumn("dbo.Investments", "projectFrom", c => c.Int(nullable: false));
            AddColumn("dbo.Entities", "entityType", c => c.String());
            DropForeignKey("dbo.Investments", "projectTo_ID", "dbo.Projects");
            DropForeignKey("dbo.Investments", "projectFrom_ID", "dbo.Projects");
            DropIndex("dbo.Investments", new[] { "projectTo_ID" });
            DropIndex("dbo.Investments", new[] { "projectFrom_ID" });
            DropColumn("dbo.Investments", "projectTo_ID");
            DropColumn("dbo.Investments", "projectFrom_ID");
        }
    }
}
