using System.ComponentModel.DataAnnotations.Schema;

namespace Models {

   [ComplexType]
   public class PersonalInfo {
      public Measurement Height { get; set; }
      public Measurement Weight { get; set; }
      public string DietryRestrictions { get; set; }
   }
}