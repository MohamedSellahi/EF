using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Model;
using DataAccess;

namespace BreakAwayConsole {
   class Program {
      static void Main(string[] args) {
         Database.SetInitializer(new InitializeBagaDatabaseWithSeedData());
         //printAllDestinations();
         //printAllDestinationsTwice();
         //printAllDestinationsOrdred();
         //printAllAustralienDestinations();
         //printDestinationNameOnly();
         //FindDestination();
         //FindGreatBarrierReef();
         //getLocalDestinationCount();
         //getLocalDestinationCountWithLoad();
         //loadAustralianDestinations();
         //localLinqQuaries();
         //ListenToLocalChanges();
         // Lazy Loading scinario
         //testLazyLoading();
         // Eager loading 
         //testEagerLoading();
         //getPrimaryContactsForAllLodgins();
         //testExplicitLoading();
         //testIsLoaded();
         //queryLodgingDistanceV1();
         //queryLodginDistaceV2();
         //queryLodginCount();

         //LoadingASubSet();

      }

      private static void LoadingASubSet() {
         using (var db = new BreakAwayContext()) {
            var canyonQuery = from d in db.Destinations
                              where d.Name == "Grand Canyon"
                              select d;

            var canyon = canyonQuery.Single();
            var lodgingQuery = db.Entry(canyon)
                                 .Collection(d => d.Lodgings)
                                 .Query()
                                 .Where(l => l.Name.Contains("Hotel"))
                                 .Select(l => l.Name);
            foreach (var item in lodgingQuery) {
               Console.WriteLine(item);
            }
         }
      }

      private static void queryLodginCount() {
         using (var db = new BreakAwayContext()) {
            var canyonQuery = from d in db.Destinations
                              where d.Name == "Grand Canyon"
                              select d;

            var canyon = canyonQuery.Single();
            var lodgingQuery = db.Entry(canyon).Collection(d => d.Lodgings).Query();

            var lodgingCount = lodgingQuery.Count();
            Console.WriteLine("Lodgins at Grand Canyon: " + lodgingCount);
         }
      }

      private static void queryLodginDistaceV2() {
         using (var db = new BreakAwayContext()) {
            var canyonQuery = from d in db.Destinations
                              where d.Name == "Grand Canyon"
                              select d;

            var canyon = canyonQuery.Single();

            var lodginQuery = db.Entry(canyon)
                                 .Collection(d => d.Lodgings)
                                 .Query();

            var distanceQuery = from l in lodginQuery
                                where l.MilesFromNearestAirport <= 10
                                select l;

            foreach (var item in distanceQuery) {
               Console.WriteLine(item.Name);
            }
         }
      }

      private static void queryLodgingDistanceV1() {
         using (var db = new BreakAwayContext()) {
            var canyonQuery = from d in db.Destinations
                              where d.Name == "Grand Canyon"
                              select d;

            var canyon = canyonQuery.Single();

            var distanceQuery = from l in canyon.Lodgings
                                where l.MilesFromNearestAirport <= 10
                                select l;

            foreach (var item in distanceQuery) {
               Console.WriteLine(item.Name);
            }
         }
      }

      private static void testIsLoaded() {
         using (var db = new BreakAwayContext()) {
            var canyon = (from d in db.Destinations
                          where d.Name == "Grand Canyon"
                          select d).Single();
            var entry = db.Entry(canyon);
            Console.WriteLine("Before load: {0}", entry.Collection(d => d.Lodgings).IsLoaded);
            entry.Collection(d => d.Lodgings).Load();
            Console.WriteLine("After load: {0}", entry.Collection(d => d.Lodgings).IsLoaded);
         }
      }

      private static void testExplicitLoading() {
         using (var db = new BreakAwayContext()) {
            var query = from d in db.Destinations
                        where d.Name == "Grand Canyon"
                        select d;

            var canyon = query.Single();
            db.Entry(canyon)
               .Collection(d => d.Lodgings)
               .Load();
            Console.WriteLine("Grand Canyon lodgings:");

            foreach (var lodg in canyon.Lodgings) {
               Console.WriteLine(lodg.Name);
            }
            // loading a person 
            var lodgin = db.Lodgings.First();
            db.Entry(lodgin).Reference(l => l.PrimaryContact).Load();
            if (lodgin.PrimaryContact != null) {
               Console.WriteLine(lodgin.PrimaryContact.FullName);
            }
         }
      }

      private static void getPrimaryContactsForAllLodgins() {
         using (var db = new BreakAwayContext()) {
            var allDestinations = db.Destinations
                     .Include(d => d.Lodgings
                                    .Select(l => l.PrimaryContact));

            foreach (Destination dest in allDestinations) {
               Console.WriteLine(dest.Name);
               foreach (var lodg in dest.Lodgings) {
                  var contact1 = lodg.PrimaryContact?.FullName;

                  if (contact1 != null) {
                     Console.WriteLine(" - " + lodg.Name + " --> Contact: " + contact1);
                  }
                  else {
                     Console.WriteLine(" - " + lodg.Name + " --> Contact: " + "None");
                  }

               }
            }
         }
      }

      private static void testEagerLoading() {
         using (var db = new BreakAwayContext()) {
            var allDestinations = db.Destinations
                                    .Include(d => d.Lodgings)
                                    ;
            foreach (Destination dest in allDestinations) {
               Console.WriteLine(dest.Name);
               foreach (var lodg in dest.Lodgings) {
                  Console.WriteLine(" - " + lodg.Name);
               }
            }
         }
      }

      private static void testLazyLoading() {
         using (var db = new BreakAwayContext()) {

            var query = from d in db.Destinations
                        where d.Name == "Grand Canyon"
                        select d;

            var canyon = query.Single();
            Console.WriteLine("Grand Canyon Loadgin:");
            if (canyon.Lodgings != null) {
               foreach (var item in canyon.Lodgings) {
                  Console.WriteLine(item.Name);
               }
            }
         }
      }

      private static void ListenToLocalChanges() {
         using (var db = new BreakAwayContext()) {
            // subscribe to events 
            db.Destinations.Local.CollectionChanged += (sender, args) => {
               if (args.NewItems != null) {
                  foreach (Destination dest in args.NewItems) {
                     Console.WriteLine("Added:" + dest.Name);
                  }
               }
               if (args.OldItems != null) {
                  foreach (Destination dest in args.OldItems) {
                     Console.WriteLine("Removed:" + dest.Name);
                  }
               }
            };

            db.Destinations.Load();
         }
      }

      private static void localLinqQuaries() {
         using (var db = new BreakAwayContext()) {
            db.Destinations.Load();

            var storedDestinations = from d in db.Destinations.Local
                                     orderby d.Name
                                     select d;

            Console.WriteLine("All Destinations: ");
            foreach (var dest in storedDestinations) {
               Console.WriteLine(dest.Name);
            }

            var aussiDestinations = from d in db.Destinations.Local
                                    where d.Country == "Australia"
                                    select d;
            Console.WriteLine();
            Console.WriteLine("Australian destinations:");
            foreach (var dest in aussiDestinations) {
               Console.WriteLine(dest.Name);
            }
         }
      }

      private static void loadAustralianDestinations() {
         using (var db = new BreakAwayContext()) {
            var query = from d in db.Destinations
                        where d.Country == "Australia"
                        select d;
            query.Load();
            //query = from d in db.Destinations
            //            where d.Country == "USA"
            //            select d;
            //query.Load();

            var count = db.Destinations.Local.Count;
            Console.WriteLine("Aussie destination in memory: {0}", count);
         }
      }

      private static void getLocalDestinationCountWithLoad() {
         using (var db = new BreakAwayContext()) {
            db.Destinations.Load();
            var count = db.Destinations.Local.Count;
            Console.WriteLine("Destinations in memory: {0}", count);
         }
      }

      private static void getLocalDestinationCount() {
         using (var db = new BreakAwayContext()) {

            foreach (Destination dest in db.Destinations) {
               Console.WriteLine(dest.Name);
            }
            var count = db.Destinations.Local.Count;
            Console.WriteLine("Destinations in memory: {0}", count);
         }
      }

      private static void FindGreatBarrierReef() {
         using (var db = new BreakAwayContext()) {
            var query = from d in db.Destinations
                        where d.Name == "Great Barrier Reef"
                        select d;
            //var reef = query.Single();
            var reef = query.SingleOrDefault();
            reef = query.FirstOrDefault();
            if (reef == null) {
               Console.WriteLine("No destination was found");
            }
            else {
               Console.WriteLine(reef.Description);
            }
         }
      }

      private static void FindDestination() {
         Console.WriteLine("Enter id Of Destination to find: ");
         try {
            var id = int.Parse(Console.ReadLine());
            using (var db = new BreakAwayContext()) {
               var destination = db.Destinations.Find(id);
               if (destination == null) {
                  Console.WriteLine("Destination not found");
               }
               else {
                  Console.WriteLine(destination.Name);
               }
            }

         }
         catch (Exception e) {
            Console.WriteLine("Retry");
         }

      }

      private static void printDestinationNameOnly() {
         using (var db = new BreakAwayContext()) {
            var query = from d in db.Destinations
                        where d.Country == "Australia"
                        orderby d.Name
                        select d.Name;
            foreach (var name in query) {
               Console.WriteLine(name);
            }
         }
      }

      private static void printAllAustralienDestinations() {
         using (var db = new BreakAwayContext()) {
            var query = from d in db.Destinations
                        where d.Country == "Australia"
                        orderby d.Name ascending
                        select d;
            var dests = db.Destinations
                          .Where(d => d.Country == "Australia")
                          .OrderByDescending(d => d.Name)
                          .Select(d => d);
            foreach (Destination dest in query) {
               Console.WriteLine(dest.Name);
            }

            foreach (Destination dest in dests) {
               Console.WriteLine(dest.Name);
            }
         }
      }

      private static void printAllDestinationsOrdred() {
         using (var db = new BreakAwayContext()) {
            var query = from d in db.Destinations
                        orderby d.Name
                        select d;

            var dests = db.Destinations.OrderBy(d => d.Name);
            foreach (Destination dest in query) {
               Console.WriteLine(dest.Name);
            }
            Console.WriteLine("==========================");
            foreach (Destination dest in dests) {
               Console.WriteLine(dest.Name);
            }
         }
      }

      private static void printAllDestinations() {
         using (BreakAwayContext db = new BreakAwayContext()) {
            foreach (var item in db.Destinations.Where(d => d.Name == "USA")) {
               Console.WriteLine(item.Name);
            }
         }
      }

      private static void printAllDestinationsTwice() {
         var allDestinations = new List<Destination>();
         using (var db = new BreakAwayContext()) {
            allDestinations = db.Destinations.ToList();
         }

         foreach (Destination dest in allDestinations) {
            Console.WriteLine(dest.Name);
         }

         foreach (Destination dest in allDestinations) {
            Console.WriteLine(dest.Name);
         }
      }
   }
}
