using System.Data.Entity;
using Model;
using MySql.Data.Entity;

namespace DataAccess {

   [DbConfigurationType(typeof(MySqlEFConfiguration))]
   public class BreakAwayContext : DbContext{
      public BreakAwayContext():base("baga") {
         
      }
      public DbSet<Destination> Destinations { get; set; }
      public DbSet<Lodging> Lodgings { get; set; }
      public DbSet<Trip> Trips { get; set; }
      public DbSet<Person> People { get; set; }
      public DbSet<Reservation> Reservations { get; set; }
      public DbSet<Payment> Payments { get; set; }
      public DbSet<Activity> Activities { get; set; }
   }
}
