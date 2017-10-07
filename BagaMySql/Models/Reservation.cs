using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models {
   public class Reservation {

      public Reservation() {
         //Payments = new List<Payment>();
      }

      [Key]
      public int ReservationId { get; set; }
      public DateTime DateTimeMade { get; set; }

      public Person Traveler { get; set; }
      //public Trip Trip { get; set; }
      public DateTime? PaidInFull { get; set; }

      //public List<Payment> Payments { get; private set; }
   }
}
