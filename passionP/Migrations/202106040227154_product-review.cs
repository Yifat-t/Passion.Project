namespace passionP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productreview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "ProductID", c => c.Int(nullable: false));
            CreateIndex("dbo.Reviews", "ProductID");
            AddForeignKey("dbo.Reviews", "ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "ProductID", "dbo.Products");
            DropIndex("dbo.Reviews", new[] { "ProductID" });
            DropColumn("dbo.Reviews", "ProductID");
        }
    }
}
