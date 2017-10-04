using System.Data.Entity;
using Model;
using System.Collections.Generic;
namespace DataAccess {
  public class InitializeBagaDatabaseWithSeedData : DropCreateDatabaseAlways<BreakAwayContext> {

      protected override void Seed(BreakAwayContext context) {
         context.Destinations.Add(
            new Destination {
               Name = "Hawaii",
               Country = "USA",
               Description = "Sunshine, beaches and fun."
            });

         context.Destinations.Add(new Destination {
            Name = "Wine Glass Bay",
            Country = "Australia",
            Description = "Picturesque sandy beaches."
         });

         context.Destinations.Add(new Destination {
            Name = "Great Barrier Reef",
            Description = "Beautiful coral reef.",
            Country = "Australia"
         });

         context.Destinations.Add(new Destination {
            Name = "Grand Canyon",
            Country = "USA",
            Description = "One huge canyon.",
            Lodgings = new List<Lodging> {
               new Lodging {
                  Name = "Grand Hotel",
                  MilesFromNearestAirport = 2.5M
               },

               new Lodging {
                  Name = "Dave's Dump",
                  MilesFromNearestAirport = 32.65M,
                  PrimaryContact = new Person {
                     FirstName = "Dave",
                     LastName = "Citizen",
                     Photo = new PersonPhoto()
                  }
               }

            }
         });

      }
   }
}
