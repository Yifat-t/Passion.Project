namespace passionP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productbrand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "BrandID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "BrandID");
            AddForeignKey("dbo.Products", "BrandID", "dbo.Brands", "BrandID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "BrandID", "dbo.Brands");
            DropIndex("dbo.Products", new[] { "BrandID" });
            DropColumn("dbo.Products", "BrandID");
        }
    }
}
