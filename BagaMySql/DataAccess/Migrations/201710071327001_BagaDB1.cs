namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BagaDB1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lodgings",
                c => new
                    {
                        LodgingId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Owner = c.String(unicode: false),
                        MilesFromNearestAirport = c.Decimal(nullable: false, precision: 18, scale: 2),
                        destination_id = c.Int(nullable: false),
                        PrimaryContactId = c.Int(),
                        SecondaryContactId = c.Int(),
                        Person_PersonId = c.Int(),
                    })
                .PrimaryKey(t => t.LodgingId)
                .ForeignKey("dbo.Destinations", t => t.destination_id, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PrimaryContactId)
                .ForeignKey("dbo.People", t => t.SecondaryContactId)
                .ForeignKey("dbo.People", t => t.Person_PersonId)
                .Index(t => t.destination_id)
                .Index(t => t.PrimaryContactId)
                .Index(t => t.SecondaryContactId)
                .Index(t => t.Person_PersonId);
            
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false, maxLength: 200, storeType: "nvarchar"),
                        Country = c.String(unicode: false),
                        Description = c.String(maxLength: 500, storeType: "nvarchar"),
                        Photo = c.Binary(storeType: "mediumblob"),
                        TravelWarnings = c.String(unicode: false),
                        ClimatInfo = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.InternetSpecials",
                c => new
                    {
                        InternetSpecialId = c.Int(nullable: false, identity: true),
                        Nights = c.Int(nullable: false),
                        CostUSD = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FromDate = c.DateTime(nullable: false, precision: 0),
                        ToDate = c.DateTime(nullable: false, precision: 0),
                        AccomodationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InternetSpecialId)
                .ForeignKey("dbo.Lodgings", t => t.AccomodationId, cascadeDelete: true)
                .Index(t => t.AccomodationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Lodgings", "Person_PersonId", "dbo.People");
            DropForeignKey("dbo.Lodgings", "SecondaryContactId", "dbo.People");
            DropForeignKey("dbo.Lodgings", "PrimaryContactId", "dbo.People");
            DropForeignKey("dbo.InternetSpecials", "AccomodationId", "dbo.Lodgings");
            DropForeignKey("dbo.Lodgings", "destination_id", "dbo.Destinations");
            DropIndex("dbo.InternetSpecials", new[] { "AccomodationId" });
            DropIndex("dbo.Lodgings", new[] { "Person_PersonId" });
            DropIndex("dbo.Lodgings", new[] { "SecondaryContactId" });
            DropIndex("dbo.Lodgings", new[] { "PrimaryContactId" });
            DropIndex("dbo.Lodgings", new[] { "destination_id" });
            DropTable("dbo.InternetSpecials");
            DropTable("dbo.Destinations");
            DropTable("dbo.Lodgings");
        }
    }
}
