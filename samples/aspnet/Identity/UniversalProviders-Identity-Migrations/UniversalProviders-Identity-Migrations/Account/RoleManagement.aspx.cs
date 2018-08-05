using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UniversalProviders_Identity_Migrations.Account
{
    public partial class RoleManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AddRoleButton_Click(object sender, EventArgs e)
        {
            if (Roles.RoleExists(AddRole.Text))
            {
                Roles.CreateRole(AddRole.Text);
            }
        }

        protected void AddUserToRole_Click(object sender, EventArgs e)
        {
            Roles.AddUserToRole(Username.Text, AddUserToRole.Text);
        }
    }
}