namespace DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BreakAwayContext : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Destinations", newName: "Locations");
            MoveTable(name: "dbo.Locations", newSchema: "baga");
        }
        
        public override void Down()
        {
            MoveTable(name: "baga.Locations", newSchema: "dbo");
            RenameTable(name: "dbo.Locations", newName: "Destinations");
        }
    }
}
