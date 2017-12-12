namespace BabyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateProductMappings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductImageMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        imageNumber = c.Int(nullable: false),
                        productId = c.Int(nullable: false),
                        productImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.productId, cascadeDelete: true)
                .ForeignKey("dbo.ProductImages", t => t.productImageId, cascadeDelete: true)
                .Index(t => t.productId)
                .Index(t => t.productImageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductImageMappings", "productImageId", "dbo.ProductImages");
            DropForeignKey("dbo.ProductImageMappings", "productId", "dbo.Products");
            DropIndex("dbo.ProductImageMappings", new[] { "productImageId" });
            DropIndex("dbo.ProductImageMappings", new[] { "productId" });
            DropTable("dbo.ProductImageMappings");
        }
    }
}
