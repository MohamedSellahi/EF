﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace BabyStore.Models {
   public class ApplicationUser: IdentityUser {

      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager) {

         var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ExternalCookie);
         return userIdentity;
      }
   }


   public class ApplicationDbContext: IdentityDbContext<ApplicationUser> {

      public ApplicationDbContext():base("DefaultConnection",throwIfV1Schema:false) {

      }

      static ApplicationDbContext() {
         // set the database initializer which is run once 
         Database.SetInitializer(new ApplicationDbInitializer());
      }

      public static ApplicationDbContext Create() {
         return new ApplicationDbContext();
      }
   }
}