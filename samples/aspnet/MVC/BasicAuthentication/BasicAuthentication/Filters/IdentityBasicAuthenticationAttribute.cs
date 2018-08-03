using System.Security.Claims;
using System.Security.Principal;
using BasicAuthentication.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BasicAuthentication.Filters
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override IPrincipal Authenticate(string userName, string password)
        {
            UserManager<IdentityUser> userManager = CreateUserManager();

            IdentityUser user = userManager.Find(userName, password);

            if (user == null)
            {
                // No user with userName/password exists.
                return null;
            }

            // Create a ClaimsIdentity with all the claims for this user.
            ClaimsIdentity identity = userManager.ClaimsIdentityFactory.CreateAsync(userManager, user, "Basic").Result;
            return new ClaimsPrincipal(identity);
        }

        private static UserManager<IdentityUser> CreateUserManager()
        {
            return new UserManager<IdentityUser>(new UserStore<IdentityUser>(new UsersDbContext()));
        }
    }
}
