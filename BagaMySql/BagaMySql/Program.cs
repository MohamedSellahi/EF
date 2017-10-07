using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using DataAccess;


namespace BagaMySql {
   class Program {
      static void Main(string[] args) {
         using (BagaDb db = new BagaDb()) {
            db.People.Add(new Person { LastName = "Sellahi", FirstName = "Mohamed" });
            db.SaveChanges();
         }
      }

   }
}


