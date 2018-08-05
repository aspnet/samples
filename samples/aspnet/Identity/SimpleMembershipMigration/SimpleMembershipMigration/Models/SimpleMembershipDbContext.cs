using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SimpleMembershipMigration.Models.Identity
{
    public class SimpleMembershipDbContext : DbContext
    {
        public SimpleMembershipDbContext()
            : this("DefaultConnection")
        { }

        public SimpleMembershipDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        public virtual IDbSet<User> Users { get; set; }

        public virtual IDbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var user = modelBuilder.Entity<User>()
                .ToTable("UserProfile");

            user.Property(u => u.Id).HasColumnName("UserId");
            user.Property(u => u.Name).HasColumnName("UserName");

            var passwordLogin = modelBuilder.Entity<PasswordLogin>()
                .HasKey(l => l.UserId)
                .ToTable("webpages_Membership");

            passwordLogin.Property(u => u.Created).HasColumnName("CreateDate");
            passwordLogin.Property(u => u.Confirmed).HasColumnName("IsConfirmed");
            passwordLogin.Property(u => u.LastLoginFailed).HasColumnName("LastPasswordFailureDate");
            passwordLogin.Property(u => u.LoginFailureCounter).HasColumnName("PasswordFailuresSinceLastSuccess");
            passwordLogin.Property(u => u.PasswordHash).HasColumnName("Password");
            passwordLogin.Property(u => u.PasswordChanged).HasColumnName("PasswordChangedDate");
            passwordLogin.Property(u => u.VerificationToken).HasColumnName("PasswordVerificationToken");
            passwordLogin.Property(u => u.VerificationTokenExpires).HasColumnName("PasswordVerificationTokenExpirationDate");

            passwordLogin.HasRequired(l => l.User).WithOptional(u => u.PasswordLogin);

            user.HasMany(u => u.Roles).WithMany(r => r.Users).Map((config) =>
            {
                config
                    .ToTable("webpages_UsersInRoles")
                    .MapLeftKey("UserId")
                    .MapRightKey("RoleId");
            });

            var externalLogin = modelBuilder.Entity<ExternalLogin>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey })
                .ToTable("webpages_OAuthMembership");

            externalLogin.Property(l => l.LoginProvider).HasColumnName("Provider");
            externalLogin.Property(l => l.ProviderKey).HasColumnName("ProviderUserId");

            externalLogin.HasRequired(l => l.User).WithMany(u => u.ExternalLogins);

            var role = modelBuilder.Entity<Role>()
                .ToTable("webpages_Roles");

            role.Property(r => r.Id).HasColumnName("RoleId");
            role.Property(r => r.Name).HasColumnName("RoleName");
        }
    }
}