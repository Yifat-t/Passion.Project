namespace passionP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class retailer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Retailers",
                c => new
                    {
                        RetailerID = c.Int(nullable: false, identity: true),
                        RetailerName = c.String(),
                    })
                .PrimaryKey(t => t.RetailerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Retailers");
        }
    }
}
