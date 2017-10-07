using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models {
   public class Person {
      public Person() {
         Address = new Address();
         Info = new PersonalInfo {
            Weight = new Measurement(),
            Height = new Measurement()
         };
      }

      [Key]
      public int PersonId { get; set; }

      public int SocialSecurityNumber { get; set; }

      public string FirstName { get; set; }
      public string LastName { get; set; }
      public PersonalInfo Info { get; private set; }
      public Address Address { get; set; }

      public string FullName { get { return FirstName + " " + LastName; } }

      public List<Lodging> PrimaryContactId { get; set; }

      [Required]
      public PersonPhoto Photo { get; set; }

      public List<Reservation> Reservations { get; set; }
   }
}
