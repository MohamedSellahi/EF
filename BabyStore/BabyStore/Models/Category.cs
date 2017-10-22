using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models {
   public class Category {
      public int ID { get; set; }

      [Display(Name="Category Name")]
      public string Name { get; set; }

      // navigation properties 
      public virtual ICollection<Product> Products { get; set; }
   }
}