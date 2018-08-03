using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace SimpleMembershipMigration.Models.Identity
{
    public class User : IUser<int>
    {
        public User()
        {
            ExternalLogins = new Collection<ExternalLogin>();
            Roles = new Collection<Role>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual PasswordLogin PasswordLogin { get; set; }

        public virtual ICollection<ExternalLogin> ExternalLogins { get; private set; }

        public virtual ICollection<Role> Roles { get; private set; }

        string IUser<int>.UserName
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }
    }

    public class PasswordLogin
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime Created { get; set; }

        public string ConfirmationToken { get; set; }

        public bool Confirmed { get; set; }

        public DateTime? LastLoginFailed { get; set; }

        public int LoginFailureCounter { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }

        public DateTime? PasswordChanged { get; set; }

        public string VerificationToken { get; set; }

        public DateTime? VerificationTokenExpires { get; set; }

    }

    public class ExternalLogin
    {
        public int UserId { get; set; }

        public virtual User User { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class Role : IRole<int>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    public class SimpleMembershipUserStore : IUserStore<User, int>, IUserLoginStore<User, int>, IUserRoleStore<User, int>,
        IUserPasswordStore<User, int>
    {
        private readonly SimpleMembershipDbContext _context;

        public SimpleMembershipUserStore(SimpleMembershipDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
        }

        public async Task CreateAsync(User user)
        {
            PasswordLogin passwordLogin = user.PasswordLogin;

            if (passwordLogin != null)
            {
                passwordLogin.Created = DateTime.Now;
                passwordLogin.Confirmed = true;

                if (passwordLogin.PasswordHash == null)
                {
                    passwordLogin.PasswordHash = String.Empty;
                }

                passwordLogin.PasswordSalt = String.Empty;
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(User user)
        {
            // This method is currently unused.
            throw new NotSupportedException();
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _context.Users.SingleOrDefaultAsync(u => u.Name == userName);
        }

        public Task UpdateAsync(User user)
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
        }

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.ExternalLogins == null)
            {
                throw new ArgumentException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            user.ExternalLogins.Add(new ExternalLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey
            });

            return Task.FromResult<object>(null);
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                return Task.FromResult<User>(null);
            }

            return _context.Users.SingleOrDefaultAsync(u => u.ExternalLogins.Any(
                l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.ExternalLogins == null)
            {
                throw new ArgumentException("user");
            }

            IList<UserLoginInfo> logins = new List<UserLoginInfo>();

            foreach (ExternalLogin login in user.ExternalLogins)
            {
                logins.Add(new UserLoginInfo(login.LoginProvider, login.ProviderKey));
            }

            return Task.FromResult(logins);
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.ExternalLogins == null)
            {
                throw new ArgumentException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            ExternalLogin toRemove = user.ExternalLogins.Single(
                l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey);
            user.ExternalLogins.Remove(toRemove);
            _context.Entry(toRemove).State = EntityState.Deleted;

            return Task.FromResult<object>(null);
        }

        public async Task AddToRoleAsync(User user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.Roles == null)
            {
                throw new ArgumentException("user");
            }

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            Role toAdd = await _context.Roles.SingleAsync(r => r.Name == role);
            user.Roles.Add(toAdd);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.Roles == null)
            {
                throw new ArgumentException("user");
            }

            IList<string> roles = new List<string>();

            foreach (Role role in user.Roles)
            {
                roles.Add(role.Name);
            }

            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(User user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.Roles == null)
            {
                throw new ArgumentException("user");
            }

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            bool inRole = user.Roles.Any(r => r.Name == role);
            return Task.FromResult(inRole);
        }

        public Task RemoveFromRoleAsync(User user, string role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.Roles == null)
            {
                throw new ArgumentException("user");
            }

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            Role toRemove = user.Roles.Single(r => r.Name == role);
            user.Roles.Remove(toRemove);

            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.PasswordLogin == null)
            {
                return Task.FromResult<string>(null);
            }

            return Task.FromResult(user.PasswordLogin.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.PasswordLogin != null);
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (user.PasswordLogin == null)
            {
                user.PasswordLogin = new PasswordLogin();
                user.PasswordLogin.Created = DateTime.UtcNow;
                user.PasswordLogin.PasswordSalt = "";
            }

            user.PasswordLogin.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }
    }

    public class SimpleMembershipRoleStore : IRoleStore<Role, int>
    {
        private readonly SimpleMembershipDbContext _context;

        public SimpleMembershipRoleStore(SimpleMembershipDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            _context = context;
        }

        public Task CreateAsync(Role role)
        {
            _context.Roles.Add(role);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Role role)
        {
            // This method is currently unused.
            throw new NotSupportedException();
        }

        public Task<Role> FindByIdAsync(int roleId)
        {
            return _context.Roles.SingleOrDefaultAsync(r => r.Id == roleId);
        }

        public Task<Role> FindByNameAsync(string roleName)
        {
            return _context.Roles.SingleOrDefaultAsync(r => r.Name == roleName);
        }

        public Task UpdateAsync(Role role)
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
        }
    }
}