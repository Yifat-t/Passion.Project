namespace passionP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pictures : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Brands", "BrandHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Brands", "PicExtension", c => c.String());
            AddColumn("dbo.Products", "ProductHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Products", "PicExtension", c => c.String());
            AddColumn("dbo.Retailers", "RetailerHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Retailers", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Retailers", "PicExtension");
            DropColumn("dbo.Retailers", "RetailerHasPic");
            DropColumn("dbo.Products", "PicExtension");
            DropColumn("dbo.Products", "ProductHasPic");
            DropColumn("dbo.Brands", "PicExtension");
            DropColumn("dbo.Brands", "BrandHasPic");
        }
    }
}
