using CustomMembershipSample.IdentityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomMembershipSample
{
    // UserManager class for this application. Used for user management
    public class AppUserManager : UserManager<AppUser, int>
    {
        public AppUserManager(IUserStore<AppUser, int> store)
            : base(store)
        {

        }

        // Create method called once during the lifecycle of request
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            // Configure the userstore to use the DbContext to work with database
            var manager = new AppUserManager(new AppUserStore(context.Get<AppDbContext>()));

            // The password validator enforces complexity on supplied password
            manager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true
            };

            // Use the custom password hasher to validate existing user credentials
            manager.PasswordHasher = new AppPasswordHasher() { DbContext = context.Get<AppDbContext>() };

            return manager;
        }
    }
}