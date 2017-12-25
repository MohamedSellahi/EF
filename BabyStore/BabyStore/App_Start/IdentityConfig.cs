using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using BabyStore.Models;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using System.Data.Entity;

namespace BabyStore {

   public class EmailService : IIdentityMessageService {

      public Task SendAsync(IdentityMessage message) {
         return Task.FromResult(0);
      }
   }

   public class SmsService : IIdentityMessageService {
      public Task SendAsync(IdentityMessage message) {
         return Task.FromResult(0);
      }
   }

   // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
   public class ApplicationUserManager : UserManager<ApplicationUser> {
      public ApplicationUserManager(IUserStore<ApplicationUser> store)
          : base(store) {
      }

      public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) {
         var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
         // Configure validation logic for usernames
         manager.UserValidator = new UserValidator<ApplicationUser>(manager) {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
         };

         // Configure validation logic for passwords
         manager.PasswordValidator = new PasswordValidator {
            RequiredLength = 6,
            RequireNonLetterOrDigit = true,
            RequireDigit = true,
            RequireLowercase = true,
            RequireUppercase = true,
         };

         // Configure user lockout defaults
         manager.UserLockoutEnabledByDefault = true;
         manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
         manager.MaxFailedAccessAttemptsBeforeLockout = 5;

         // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
         // You can write your own provider and plug it in here.
         manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser> {
            MessageFormat = "Your security code is {0}"
         });
         manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser> {
            Subject = "Security Code",
            BodyFormat = "Your security code is {0}"
         });
         manager.EmailService = new EmailService();
         manager.SmsService = new SmsService();
         var dataProtectionProvider = options.DataProtectionProvider;
         if (dataProtectionProvider != null) {
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
         }
         return manager;
      }
   }

   // Configure the application sign-in manager which is used in this application.
   public class ApplicationSignInManager : SignInManager<ApplicationUser, string> {
      public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
          : base(userManager, authenticationManager) {
      }

      public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user) {
         return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
      }

      public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context) {
         return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
      }
   }

   // Role manager 
   public class ApplicationRoleManager: RoleManager<IdentityRole> {

      public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore):base(roleStore) {

      }

      // 
      public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context) {
         return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
      }

   }

   // Seed data base for managing roles 
   public class ApplicationDbInitializer: DropCreateDatabaseIfModelChanges<ApplicationDbContext> {
      protected override void Seed(ApplicationDbContext context) {
         InitializeIdentityForEF(context);
         base.Seed(context);
      }

      //Create User=admin@mvcbabystore.com with Adm1n@mvcbabystore.com in the Admin role
      private void InitializeIdentityForEF(ApplicationDbContext context) {
         var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
         var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
         const string name = "admin@mvcbabystore.com";
         const string password = "Adm1n@mvcbabystore.com";
         const string roleName = "Admin";

         // create a role admin if it does not exist 
         var role = roleManager.FindByName(roleName);
         if (role == null) {
            role = new IdentityRole(roleName);
            var roleResult = roleManager.Create(role);
         }

         var user = userManager.FindByName(name);
         if (user == null) {
            user = new ApplicationUser {
               UserName = name,
               Email = name
            };
            var result = userManager.Create(user, password);
            result = userManager.SetLockoutEnabled(user.Id, false);
         }

         // add user admin to role admin if not already added 
         var rolesForUser = userManager.GetRoles(user.Id);
         if (!rolesForUser.Contains(role.Name)) {
            var result = userManager.AddToRole(user.Id, role.Name);
         }
      }
   }


}