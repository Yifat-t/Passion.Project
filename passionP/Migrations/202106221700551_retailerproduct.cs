namespace passionP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class retailerproduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.RetailerProducts", "Retailer_RetailerID", "dbo.Retailers");
            DropForeignKey("dbo.RetailerProducts", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.RetailerProducts", new[] { "Retailer_RetailerID" });
            DropIndex("dbo.RetailerProducts", new[] { "Product_ProductID" });
            
            DropTable("dbo.RetailerProducts");

            CreateTable(
                "dbo.RetailerProducts",
                c => new
                    {
                        RetailerID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RetailerID, t.ProductID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Retailers", t => t.RetailerID, cascadeDelete: true)
                .Index(t => t.RetailerID)
                .Index(t => t.ProductID);
            
            
        }
        
        public override void Down()
        {
            DropTable("dbo.RetailerProducts");

            CreateTable(
                "dbo.RetailerProducts",
                c => new
                    {
                        Retailer_RetailerID = c.Int(nullable: false),
                        Product_ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Retailer_RetailerID, t.Product_ProductID });
            
            DropForeignKey("dbo.RetailerProducts", "RetailerID", "dbo.Retailers");
            DropForeignKey("dbo.RetailerProducts", "ProductID", "dbo.Products");
            DropIndex("dbo.RetailerProducts", new[] { "ProductID" });
            DropIndex("dbo.RetailerProducts", new[] { "RetailerID" });
            
            CreateIndex("dbo.RetailerProducts", "Product_ProductID");
            CreateIndex("dbo.RetailerProducts", "Retailer_RetailerID");
            AddForeignKey("dbo.RetailerProducts", "Product_ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
            AddForeignKey("dbo.RetailerProducts", "Retailer_RetailerID", "dbo.Retailers", "RetailerID", cascadeDelete: true);
        }
    }
}
