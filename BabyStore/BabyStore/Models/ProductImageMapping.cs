using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyStore.Models {
   public class ProductImageMapping {
      public int Id { get; set; }
      public int imageNumber { get; set; }
      public int productId { get; set; }
      public int productImageId { get; set; }

      // defining the many to many relationship
      public virtual Product Product { get; set; }
      public virtual ProductImage productImage { get; set; }
   }
}