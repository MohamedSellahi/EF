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
         FindGreatBarrierReef();

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
