using BabyStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BabyStore.DAL {
   public class StoreContext:DbContext {
      public StoreContext():base("BabyStore") {}
      public DbSet<Product> Products { get; set; }
      public DbSet<Category> Categories { get; set; }
   }
}