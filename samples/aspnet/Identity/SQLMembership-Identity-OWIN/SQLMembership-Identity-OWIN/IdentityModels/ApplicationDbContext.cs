using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace SQLMembership_Identity_OWIN
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext()
            : base("ApplicationServices")
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
            application.HasKey(t => t.ApplicationId).ToTable("aspnet_Applications");

        }

        public virtual IDbSet<Application> Applications { get; set; }
    }
}