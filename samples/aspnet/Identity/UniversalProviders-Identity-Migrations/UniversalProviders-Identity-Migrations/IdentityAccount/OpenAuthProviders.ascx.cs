using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using UniversalProviders_Identity_Migrations;

namespace UniversalProviders_Identity_Migrations.IdentityAccount
{
    public partial class OpenAuthProviders : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var provider = Request.Form["provider"];
            if (provider == null)
            {
                return;
            }
            // Request a redirect to the external login provider
            string redirectUrl = ResolveUrl(String.Format(CultureInfo.InvariantCulture, "~/IdentityAccount/RegisterExternalLogin.aspx?{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl));
            var properties = new AuthenticationProperties() { RedirectUri = redirectUrl };
            // Add xsrf verification when linking accounts
            if (Context.User.Identity.IsAuthenticated)
            {
                properties.Dictionary[IdentityHelper.XsrfKey] = Context.User.Identity.GetUserId();
            }
            Context.GetOwinContext().Authentication.Challenge(properties, provider);
            Response.StatusCode = 401;
            Response.End();
        }

        public string ReturnUrl { get; set; }

        public IEnumerable<string> GetProviderNames()
        {
            return Context.GetOwinContext().Authentication.GetExternalAuthenticationTypes().Select(t => t.AuthenticationType);
        }
    }
}