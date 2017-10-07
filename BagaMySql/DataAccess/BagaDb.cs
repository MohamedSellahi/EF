using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace DataAccess {

   public class BagaDb:DbContext {
      public BagaDb():base("baga") {

      }

      public DbSet<Person> People { get; set; }
   }
}
