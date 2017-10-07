using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models {
   public class Payment {
      public Payment() {
         PaymentDate = DateTime.Now;
      }

      public int PaymentId { get; set; }
      public int ReservationId { get; set; }
      public DateTime PaymentDate { get; set; }
      public decimal Amount { get; set; }
   }
}
