namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activities",
                c => new
                    {
                        ActivityId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ActivityId);
            
            CreateTable(
                "dbo.Trips",
                c => new
                    {
                        Identifier = c.Guid(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        CostUSD = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        DestinationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Identifier)
                .ForeignKey("dbo.Destinations", t => t.DestinationId, cascadeDelete: true)
                .Index(t => t.DestinationId);
            
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        LocationID = c.Int(nullable: false, identity: true),
                        LocationName = c.String(nullable: false, maxLength: 200),
                        Country = c.String(),
                        Description = c.String(maxLength: 500),
                        Photo = c.Binary(maxLength: 8000),
                        TravelWarnings = c.String(),
                        ClimatInfo = c.String(),
                    })
                .PrimaryKey(t => t.LocationID);
            
            CreateTable(
                "dbo.Lodgings",
                c => new
                    {
                        LodgingId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Owner = c.String(),
                        MilesFromNearestAirport = c.Decimal(nullable: false, precision: 18, scale: 2),
                        destination_id = c.Int(nullable: false),
                        PrimaryContactId = c.Int(),
                        SecondaryContactId = c.Int(),
                        MaxPersonsPerRoom = c.Int(),
                        PrivateRoomsAvailable = c.Boolean(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.LodgingId)
                .ForeignKey("dbo.Destinations", t => t.destination_id, cascadeDelete: true)
                .ForeignKey("dbo.People", t => t.PrimaryContactId)
                .ForeignKey("dbo.People", t => t.SecondaryContactId)
                .Index(t => t.destination_id)
                .Index(t => t.PrimaryContactId)
                .Index(t => t.SecondaryContactId);
            
            CreateTable(
                "dbo.InternetSpecials",
                c => new
                    {
                        InternetSpecialId = c.Int(nullable: false, identity: true),
                        Nights = c.Int(nullable: false),
                        CostUSD = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        AccomodationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InternetSpecialId)
                .ForeignKey("dbo.Lodgings", t => t.AccomodationId, cascadeDelete: true)
                .Index(t => t.AccomodationId);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        ParsonId = c.Int(nullable: false, identity: true),
                        SocialSecurityNumber = c.Int(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Info_Height_Reading = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Info_Height_Units = c.String(),
                        Info_Weight_Reading = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Info_Weight_Units = c.String(),
                        Info_DietryRestrictions = c.String(),
                        Address_AddressId = c.Int(nullable: false),
                        StreetAddress = c.String(maxLength: 50),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                    })
                .PrimaryKey(t => t.ParsonId);
            
            CreateTable(
                "dbo.PersonPhotoes",
                c => new
                    {
                        personId = c.Int(nullable: false),
                        Photo = c.Binary(maxLength: 8000),
                        Caption = c.String(),
                    })
                .PrimaryKey(t => t.personId)
                .ForeignKey("dbo.People", t => t.personId)
                .Index(t => t.personId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        ReservationId = c.Int(nullable: false, identity: true),
                        DateTimeMade = c.DateTime(nullable: false),
                        PaidInFull = c.DateTime(),
                        Traveler_ParsonId = c.Int(),
                        Trip_Identifier = c.Guid(),
                    })
                .PrimaryKey(t => t.ReservationId)
                .ForeignKey("dbo.People", t => t.Traveler_ParsonId)
                .ForeignKey("dbo.Trips", t => t.Trip_Identifier)
                .Index(t => t.Traveler_ParsonId)
                .Index(t => t.Trip_Identifier);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        PaymentId = c.Int(nullable: false, identity: true),
                        ReservationId = c.Int(nullable: false),
                        PaymentDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PaymentId)
                .ForeignKey("dbo.Reservations", t => t.ReservationId, cascadeDelete: true)
                .Index(t => t.ReservationId);
            
            CreateTable(
                "dbo.TripActivities",
                c => new
                    {
                        Trip_Identifier = c.Guid(nullable: false),
                        Activity_ActivityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Trip_Identifier, t.Activity_ActivityId })
                .ForeignKey("dbo.Trips", t => t.Trip_Identifier, cascadeDelete: true)
                .ForeignKey("dbo.Activities", t => t.Activity_ActivityId, cascadeDelete: true)
                .Index(t => t.Trip_Identifier)
                .Index(t => t.Activity_ActivityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Trips", "DestinationId", "dbo.Destinations");
            DropForeignKey("dbo.Lodgings", "SecondaryContactId", "dbo.People");
            DropForeignKey("dbo.Lodgings", "PrimaryContactId", "dbo.People");
            DropForeignKey("dbo.Reservations", "Trip_Identifier", "dbo.Trips");
            DropForeignKey("dbo.Reservations", "Traveler_ParsonId", "dbo.People");
            DropForeignKey("dbo.Payments", "ReservationId", "dbo.Reservations");
            DropForeignKey("dbo.PersonPhotoes", "personId", "dbo.People");
            DropForeignKey("dbo.InternetSpecials", "AccomodationId", "dbo.Lodgings");
            DropForeignKey("dbo.Lodgings", "destination_id", "dbo.Destinations");
            DropForeignKey("dbo.TripActivities", "Activity_ActivityId", "dbo.Activities");
            DropForeignKey("dbo.TripActivities", "Trip_Identifier", "dbo.Trips");
            DropIndex("dbo.TripActivities", new[] { "Activity_ActivityId" });
            DropIndex("dbo.TripActivities", new[] { "Trip_Identifier" });
            DropIndex("dbo.Payments", new[] { "ReservationId" });
            DropIndex("dbo.Reservations", new[] { "Trip_Identifier" });
            DropIndex("dbo.Reservations", new[] { "Traveler_ParsonId" });
            DropIndex("dbo.PersonPhotoes", new[] { "personId" });
            DropIndex("dbo.InternetSpecials", new[] { "AccomodationId" });
            DropIndex("dbo.Lodgings", new[] { "SecondaryContactId" });
            DropIndex("dbo.Lodgings", new[] { "PrimaryContactId" });
            DropIndex("dbo.Lodgings", new[] { "destination_id" });
            DropIndex("dbo.Trips", new[] { "DestinationId" });
            DropTable("dbo.TripActivities");
            DropTable("dbo.Payments");
            DropTable("dbo.Reservations");
            DropTable("dbo.PersonPhotoes");
            DropTable("dbo.People");
            DropTable("dbo.InternetSpecials");
            DropTable("dbo.Lodgings");
            DropTable("dbo.Destinations");
            DropTable("dbo.Trips");
            DropTable("dbo.Activities");
        }
    }
}
