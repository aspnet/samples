using System;
using System.Linq;
using System.Threading.Tasks;
using SQLMembership_Identity_OWIN;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;
using System.Configuration;

namespace SQLMembership_Identity_OWIN.IdentityAcccount
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
            User user = new User() { UserName = Username.Text,ApplicationId=currentApplicationId, LoweredUserName=Username.Text.ToLower()};

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
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString))
            {
                string queryString = "SELECT ApplicationId from aspnet_Applications WHERE ApplicationName = '/'"; //Set application name as in database

                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    return reader.GetGuid(0);
                }
                connection.Close();
                return createApplication(connection, "/");
            }
        }

        private Guid createApplication(SqlConnection sqlConnection, string s)
        {
            var appid = Guid.NewGuid();
            var insStatement =
                $"INSERT INTO [dbo].aspnet_Applications" +
                $"([ApplicationName], [LoweredApplicationName], [ApplicationId], [Description]) " +
                $"VALUES (@appName, @lowAppName, @appId, 'SQLMembership-Identity-OWIN')";

            var sqlcmd = new SqlCommand(insStatement, sqlConnection);
            sqlcmd.Connection.Open();

            sqlcmd.Parameters.AddWithValue("@appId", appid);
            sqlcmd.Parameters.AddWithValue("@appName", s);
            sqlcmd.Parameters.AddWithValue("@lowAppName",
                (s ?? "").ToLower());

            var rc = sqlcmd.ExecuteNonQuery();
            Console.WriteLine($"insert application returns {rc}");
            return appid;
        }
    }
}