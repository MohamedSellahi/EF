using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models {
   public class Activity {
      public int ActivityId { get; set; }

      [Required, MaxLength(50)]
      public string Name { get; set; }
      public List<Trip> Trips { get; set; }
   }
}
