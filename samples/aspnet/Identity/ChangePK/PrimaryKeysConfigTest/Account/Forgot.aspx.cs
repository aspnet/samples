using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using System;
using System.Web;
using System.Web.UI;
using PrimaryKeysConfigTest.Models;
using Owin;

namespace PrimaryKeysConfigTest.Account
{
    public partial class ForgotPassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Forgot(object sender, EventArgs e)
        {
            if (IsValid)
            {
                // Validate the user password
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                ApplicationUser user = manager.FindByName(Email.Text);
                if (user == null || !manager.IsEmailConfirmed<ApplicationUser,int>(user.Id))
                {
                    FailureText.Text = "The user either does not exist or is not confirmed.";
                    ErrorMessage.Visible = true;
                    return;
                }
                // PLACEHOLDER: Add fwlink: send email with the code and the redirect to reset password page
                // PLEASE NOTE: This is an insecure way of sending a reset password token. 
                // The ideally approach is to send an email link to the user and confirm it. 
                // THIS FLOW IS FOR DEMO PURPOSES ONLY

                 var code = manager.GeneratePasswordResetToken<ApplicationUser,int>(user.Id);
                 Response.Redirect(IdentityHelper.GetResetPasswordRedirectUrl(code));
            }
        }
    }
}