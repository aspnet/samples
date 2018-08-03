using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using UniversalProviders_Identity_Migrations.IdentityModels;

namespace UniversalProviders_Identity_Migrations
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            this.Database.Log = Logger;
        }

        private void Logger(string log)
        {
            Debug.WriteLine(log);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var application = modelBuilder.Entity<Application>();
            application.HasKey(t => t.ApplicationId).ToTable("Applications");

            modelBuilder.Entity<UserDbProfile>().ToTable("Profiles");

        }

        public virtual IDbSet<Application> Applications { get; set; }
        public virtual IDbSet<UserDbProfile> Profiles { get; set; }
    }
}