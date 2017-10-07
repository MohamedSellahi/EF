namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BagaDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PersonPhotoes",
                c => new
                    {
                        personId = c.Int(nullable: false),
                        Photo = c.Binary(storeType: "mediumblob"),
                        Caption = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.personId)
                .ForeignKey("dbo.People", t => t.personId)
                .Index(t => t.personId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonPhotoes", "personId", "dbo.People");
            DropIndex("dbo.PersonPhotoes", new[] { "personId" });
            DropTable("dbo.PersonPhotoes");
        }
    }
}
