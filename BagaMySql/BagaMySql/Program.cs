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
            var p = db.People.Find(1);
            db.People.Remove(p);
            db.SaveChanges();
         }

      }
   }
}

