using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.UI;
using PrimaryKeysConfigTest.Models;
using Owin;

namespace PrimaryKeysConfigTest.Account
{
    public partial class Confirm : Page
    {
        protected string StatusMessage
        {
            get;
            private set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string code = IdentityHelper.GetCodeFromRequest(Request);
            string userId = IdentityHelper.GetUserIdFromRequest(Request);
            if (code != null && userId != null)
            {
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var result = manager.ConfirmEmail<ApplicationUser,int>(Int32.Parse(userId), code);
                if (result.Succeeded)
                {
                    StatusMessage = "Thank you for confirming your account.";
                    return;
                }
            }

            StatusMessage = "An error has occurred";
        }
    }
}