namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvestmentDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Investments", "description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Investments", "description");
        }
    }
}
