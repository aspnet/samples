using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BasicAuthentication.Data
{
    public class UsersDbContext : IdentityDbContext<IdentityUser>
    {
        static UsersDbContext()
        {
            Database.SetInitializer(new Initializer());
        }

        private class Initializer : CreateDatabaseIfNotExists<UsersDbContext>
        {
            protected override void Seed(UsersDbContext context)
            {
                IdentityRole role = context.Roles.Add(new IdentityRole("User"));

                IdentityUser user = new IdentityUser("SampleUser");
                user.Roles.Add(new IdentityUserRole { Role = role });
                user.Claims.Add(new IdentityUserClaim
                {
                    ClaimType = "hasRegistered",
                    ClaimValue = "true"
                });

                user.PasswordHash = new PasswordHasher().HashPassword("secret");
                context.Users.Add(user);
                context.SaveChanges();
                base.Seed(context);
            }
        }
    }
}
