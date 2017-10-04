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
         using (BreakAwayContext db = new BreakAwayContext()) {
            foreach (var item in db.Destinations) {
               Console.WriteLine(item.Country);
            }
         }
      }
   }
}
