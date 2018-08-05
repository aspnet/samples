using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using PrimaryKeysConfigTest.Models;

namespace PrimaryKeysConfigTest.Account
{
    public partial class Register : Page
    {
        protected void CreateUser_Click(object sender, EventArgs e)
        {
            var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var user = new ApplicationUser() { UserName = Email.Text, Email = Email.Text, MyAddress = new Address() { City = "Redmond", State = "WA" } };
            IdentityResult result = manager.Create(user, Password.Text);
            if (result.Succeeded)
            {
                IdentityHelper.SignIn(manager, user, isPersistent: false);

                // PLEASE NOTE: This is an insecure way of confirming a user. This is for demo purpose only
                // The ideally approach is to send an email link to the user and confirm it. 
                // THIS FLOW IS FOR DEMO PURPOSES ONLY

                 var code = manager.GenerateEmailConfirmationToken<ApplicationUser,int>(user.Id);
                 Response.Redirect(IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id.ToString()));

                IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
            }
            else 
            {
                ErrorMessage.Text = result.Errors.FirstOrDefault();
            }
        }
    }
}