using System.ComponentModel.DataAnnotations.Schema;

namespace Model {
   [ComplexType]
   public class PersonalInfo {
      public Measurement Height { get; set; }
      public Measurement Weight { get; set; }
      public string DietryRestrictions { get; set; }
   }
}