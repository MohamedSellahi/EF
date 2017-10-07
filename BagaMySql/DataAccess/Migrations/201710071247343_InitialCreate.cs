namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        PersonId = c.Int(nullable: false, identity: true),
                        SocialSecurityNumber = c.Int(nullable: false),
                        FirstName = c.String(unicode: false),
                        LastName = c.String(unicode: false),
                        Info_Height_Reading = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Info_Height_Units = c.String(unicode: false),
                        Info_Weight_Reading = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Info_Weight_Units = c.String(unicode: false),
                        Info_DietryRestrictions = c.String(unicode: false),
                        Address_AddressId = c.Int(nullable: false),
                        StreetAddress = c.String(maxLength: 50, storeType: "nvarchar"),
                        City = c.String(unicode: false),
                        State = c.String(unicode: false),
                        ZipCode = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.People");
        }
    }
}
