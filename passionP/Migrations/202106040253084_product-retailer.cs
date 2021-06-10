namespace passionP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class productretailer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RetailerProducts",
                c => new
                    {
                        Retailer_RetailerID = c.Int(nullable: false),
                        Product_ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Retailer_RetailerID, t.Product_ProductID })
                .ForeignKey("dbo.Retailers", t => t.Retailer_RetailerID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ProductID, cascadeDelete: true)
                .Index(t => t.Retailer_RetailerID)
                .Index(t => t.Product_ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RetailerProducts", "Product_ProductID", "dbo.Products");
            DropForeignKey("dbo.RetailerProducts", "Retailer_RetailerID", "dbo.Retailers");
            DropIndex("dbo.RetailerProducts", new[] { "Product_ProductID" });
            DropIndex("dbo.RetailerProducts", new[] { "Retailer_RetailerID" });
            DropTable("dbo.RetailerProducts");
        }
    }
}
