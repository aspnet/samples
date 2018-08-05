using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;

namespace Owin
{
    public static class AppBuilderExtensions
    {
        public static void UseExternalSignInCookie(this IAppBuilder app, CookieAuthenticationOptions cookieAuthenticationOptions)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            string externalAuthenticationType = cookieAuthenticationOptions.AuthenticationType;
            app.SetDefaultSignInAsAuthenticationType(externalAuthenticationType);
            cookieAuthenticationOptions.AuthenticationMode = AuthenticationMode.Passive;
            cookieAuthenticationOptions.CookieName = ".AspNet." + externalAuthenticationType;
            cookieAuthenticationOptions.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
            CookieAuthenticationExtensions.UseCookieAuthentication(app, cookieAuthenticationOptions);
        }

        public static void UseExternalSignInCookie(this IAppBuilder app, string externalAuthenticationType, bool isPersistent = false)
        {
            CookieAuthenticationOptions cookieAuthenticationOptions = new CookieAuthenticationOptions();
            cookieAuthenticationOptions.AuthenticationType = externalAuthenticationType;
            cookieAuthenticationOptions.Provider = new CookieAuthenticationProvider() { OnResponseSignIn = c => c.Properties.IsPersistent = isPersistent };
            app.UseExternalSignInCookie(cookieAuthenticationOptions);
        }
    }
}