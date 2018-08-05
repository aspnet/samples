using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomMembershipSample.IdentityModels
{
    // Define user class that extends from Identity
    public class AppUser : IdentityUser<int, AppLogin, AppUserRole, AppClaim>
    {
        public AppUser()
            : base()
        {
            this.Addresses = new HashSet<Address>();
            DateCreated = DateTime.Now;
            LastActivityDate = DateTime.Now;
        }

        public override int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public override string Email { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.DateTime> LastActivityDate { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }

        // map to existing attributes
        public override string PasswordHash
        {
            get
            {
                return Password;
            }
            set
            {
                Password = value;
            }
        }

        // Map to existing username 
        public override string UserName
        {
            get
            {
                return Username;
            }
            set
            {
                Username = value;
            }
        }

        public Task<ClaimsIdentity> GenerateUserIdentity(AppUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity<AppUser, int>(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return Task.FromResult(userIdentity);
        }

    }

    public class AppLogin : IdentityUserLogin<int>
    {
    }

    public class AppClaim : IdentityUserClaim<int>
    {
    }

    public class AppRole : IdentityRole<int, AppUserRole>
    {
        public AppRole()
            : base()
        {

        }

        public AppRole(string name)
            : base()
        {
            Name = name;
        }
    }

    public class AppUserRole : IdentityUserRole<int>
    {
        public AppUserRole()
            : base()
        {

        }

    }

    public class AppUserStore : UserStore<AppUser, AppRole, int, AppLogin, AppUserRole, AppClaim>
    {
        public AppUserStore(AppDbContext context)
            : base(context)
        {
        }
    }

    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }

        public int UserId { get; set; }

        public virtual AppUser AppUser { get; set; }
    }

    // Custom password hasher class. This is to reuse exisitng hashed credentials
    public class AppPasswordHasher : PasswordHasher
    {
        public AppDbContext DbContext { get; set; }

        // Custom hashing used before migrating to Identity
        public static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        // Verify if the password is hashed using MD5. If yes, rehash using ASP.NET Identity Crypto which is more secure
        // this is invoked when old users try to login. Eventually all the old the password are rehashed to a more secure hash
        public override PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            if (String.Equals(hashedPassword, GetMD5Hash(providedPassword), StringComparison.InvariantCultureIgnoreCase))
            {
                ReHashPassword(hashedPassword, providedPassword);
                return PasswordVerificationResult.Success;
            }

            return base.VerifyHashedPassword(hashedPassword, providedPassword);
        }

        // Rehash password using ASP.NET Identity Crypto
        // Store it back into database
        private void ReHashPassword(string hashedPassword, string providedPassword)
        {

            var user = DbContext.Users.Where(x => x.PasswordHash == hashedPassword).FirstOrDefault();

            user.PasswordHash = base.HashPassword(providedPassword);

            // Update SecurityStamp with new Guid to nullify any previous cookies
            user.SecurityStamp = Guid.NewGuid().ToString();

            DbContext.SaveChanges();
        }
    }
}