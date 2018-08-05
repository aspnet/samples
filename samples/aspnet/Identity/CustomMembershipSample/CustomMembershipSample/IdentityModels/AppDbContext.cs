using CustomMembershipSample;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CustomMembershipSample.IdentityModels
{
    // Extend DbContext for Identity to get pre defined mapping and properties
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, AppLogin, AppUserRole, AppClaim>
    {
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        public AppDbContext()
            : base("AppDbConnection")
        {
            Database.Log = (str) => { Debug.WriteLine(str); };
        }

        // Custom mapping for properties. Mapping will be different based on 
        // existing custom database
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Map users to existing AppUsers table
            modelBuilder.Entity<AppUser>().ToTable("AppUsers");

            // Ignore columns that are not needed
            // You might want to include them if you want to use features like
            // Two factor authentication, email confirmation etc
            var entity = modelBuilder.Entity<AppUser>();
            entity.HasKey(x => x.Id);
            entity.Ignore(x => x.Password);
            entity.Ignore(x => x.Username);
            entity.Ignore(x => x.EmailConfirmed);
            entity.Ignore(x => x.PhoneNumber);
            entity.Ignore(x => x.PhoneNumberConfirmed);
            entity.Ignore(x => x.TwoFactorEnabled);
            entity.Ignore(x => x.LockoutEnabled);
            entity.Ignore(x => x.LockoutEndDateUtc);
            entity.Ignore(x => x.AccessFailedCount);

            entity.Property(x => x.UserName).HasColumnName("Username");
            entity.Property(x => x.PasswordHash).HasColumnName("Password");

            entity.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            entity.HasMany(u => u.Addresses).WithRequired(x=>x.AppUser).HasForeignKey(ur => ur.UserId);
        }

        public DbSet<Address> Addresses { get; set; }
    }


}