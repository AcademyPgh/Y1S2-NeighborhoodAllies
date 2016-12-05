namespace ImpactMap.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetricResult : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Metrics", "Report_ID", "dbo.Reports");
            DropIndex("dbo.Metrics", new[] { "Report_ID" });
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
            
            DropColumn("dbo.Metrics", "Report_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Metrics", "Report_ID", c => c.Int());
            DropForeignKey("dbo.MetricResults", "report_ID", "dbo.Reports");
            DropForeignKey("dbo.MetricResults", "metric_ID", "dbo.Metrics");
            DropIndex("dbo.MetricResults", new[] { "report_ID" });
            DropIndex("dbo.MetricResults", new[] { "metric_ID" });
            DropTable("dbo.MetricResults");
            CreateIndex("dbo.Metrics", "Report_ID");
            AddForeignKey("dbo.Metrics", "Report_ID", "dbo.Reports", "ID");
        }
    }
}
