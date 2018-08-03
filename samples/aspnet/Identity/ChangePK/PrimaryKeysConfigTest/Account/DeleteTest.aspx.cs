using PrimaryKeysConfigTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Owin;

namespace PrimaryKeysConfigTest.Account
{
    public partial class DeleteTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DeleteUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();

            var result = manager.Delete(manager.FindByName(username.Text));

            if (!result.Succeeded)
            {
                throw new Exception("user not deleted");
            }
        }

        protected void DeleteRole_Click(object sender, EventArgs e)
        {
            var manager = new RoleManager<CustomRole,int>(new CustomRoleStore(new ApplicationDbContext()));

            manager.Create(new CustomRole() { Name = Rolename.Text });

            var result =  manager.Delete(manager.FindByName(Rolename.Text));

            if (!result.Succeeded)
            {
                throw new Exception("role not deleted");
            }
        }
    }
}