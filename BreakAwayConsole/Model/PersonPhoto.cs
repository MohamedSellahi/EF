using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model {
   public class PersonPhoto {
      [Key]
      [ForeignKey("PhotoOf")]
      public int personId { get; set; }

      [Column(TypeName ="varbinary")]
      public byte[] Photo { get; set; }
      public string Caption { get; set; }

      public Person PhotoOf { get; set; }

   }
}