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
         var init = new InitializeBagaDatabaseWithSeedData();
         //Database.SetInitializer(init);
         using (BreakAwayContext db = new BreakAwayContext()) {
            db.People.Add(new Person()
            {
               FirstName = "mohamed",
               LastName = "sellahi",
               
            });

            //foreach (var item in db.Destinations) {
            //   Console.WriteLine(item.Country);
            //}
         }
      }
   }
}
