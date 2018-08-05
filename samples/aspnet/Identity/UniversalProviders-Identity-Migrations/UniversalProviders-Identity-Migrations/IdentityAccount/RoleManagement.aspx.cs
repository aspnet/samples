using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniversalProviders_Identity_Migrations;

namespace UniversalProviders_Identity_Migrations.IdentityAccount
{
    public partial class RoleManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AddRoleButton_Click(object sender, EventArgs e)
        {
            var manager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!manager.RoleExists(AddRole.Text))
            {
                var result = manager.Create(new IdentityRole() { Name = AddRole.Text });
                if (result.Succeeded)
                {
                    Response.Write("Done");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault());
                }
            }
        }

        protected void AddUserRole_Click(object sender, EventArgs e)
        {
            var manager = new UserManager();
            var user = manager.FindByName(Username.Text);

            if (user == null)
            {
                ModelState.AddModelError("", "No user found");
                return;
            }

            var result = manager.AddToRole(user.Id, Rolename.Text);

            if (result.Succeeded)
            {
                Response.Write("Done");
            }
            else
            {
                ModelState.AddModelError("", result.Errors.FirstOrDefault());
            }
        }
    }
}