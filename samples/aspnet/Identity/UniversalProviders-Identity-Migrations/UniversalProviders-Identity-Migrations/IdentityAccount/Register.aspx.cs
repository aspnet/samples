using System;
using System.Linq;
using System.Threading.Tasks;
using UniversalProviders_Identity_Migrations;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Configuration;

namespace UniversalProviders_Identity_Migrations.IdentityAcccount
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            var currentApplicationId = GetApplicationID();

            var manager = new UserManager();
            User user = new User() { UserName = Username.Text,ApplicationId=currentApplicationId};

            user.IsApproved = true;

            var result = manager.Create(user, Password.Text);

            if (result.Succeeded)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);
                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else
            {
                ModelState.AddModelError("", result.Errors.FirstOrDefault());
            }
        }

        private Guid GetApplicationID()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string queryString = "SELECT ApplicationId from Applications WHERE ApplicationName = '/'"; //Set application name as in database

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return reader.GetGuid(0);
                }

                return Guid.NewGuid();
            }
        }
    }
}