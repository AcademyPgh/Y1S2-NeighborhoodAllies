namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tagsandcategories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Metrics", "Investment_ID", c => c.Int());
            CreateIndex("dbo.Metrics", "Investment_ID");
            AddForeignKey("dbo.Metrics", "Investment_ID", "dbo.Investments", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Metrics", "Investment_ID", "dbo.Investments");
            DropIndex("dbo.Metrics", new[] { "Investment_ID" });
            DropColumn("dbo.Metrics", "Investment_ID");
        }
    }
}
