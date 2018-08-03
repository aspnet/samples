using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Configuration;
using Todo.Web.Providers;

namespace Todo.Web
{
    public partial class Startup
    {
        static Startup()
        {
            PublicClientId = "self";

            UserManagerFactory = () => new UserManager<IdentityUser>(new UserStore<IdentityUser>(new IdentityDbContext("TodosDbContext")));

            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId, UserManagerFactory),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };
        }

        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static Func<UserManager<IdentityUser>> UserManagerFactory { get; set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie, isPersistent: true);

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            string microsoftClientId = ConfigurationManager.AppSettings["microsoft_clientid"];
            string microsoftClientSecret = ConfigurationManager.AppSettings["microsoft_clientsecret"];
            if (microsoftClientId != null && microsoftClientSecret != null)
            {
                app.UseMicrosoftAccountAuthentication(microsoftClientId, microsoftClientSecret);
            }

            string twitterConsumerKey = ConfigurationManager.AppSettings["twitter_consumerkey"];
            string twitterConsumerSecret = ConfigurationManager.AppSettings["twitter_consumersecret"];
            if (twitterConsumerKey != null && twitterConsumerSecret != null)
            {
                app.UseTwitterAuthentication(twitterConsumerKey, twitterConsumerSecret);
            }

            string fbAppId = ConfigurationManager.AppSettings["facebook_appid"];
            string fbAppSecret = ConfigurationManager.AppSettings["facebook_appsecret"];
            if (fbAppId != null && fbAppSecret != null)
            {
                app.UseFacebookAuthentication(fbAppId, fbAppSecret);
            }

            app.UseGoogleAuthentication();
        }
    }
}
