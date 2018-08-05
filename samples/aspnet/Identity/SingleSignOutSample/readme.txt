ASP.NET Identity sample app
Web application to demonstrate single signout everywhere.

To run the sample, follow the following steps:

1. Run the app in IE
2. Register a new user called a@a.com
3. Open the app in other browers like chrome or firefox
4. Login with a@a.com account in new browser
5. Refresh both browsers' home page, both show registered clients (IE, Chrome) of the account
Note: use browser user agent name as client here is not a common practice. It's used for demo purpose.
In real use case, it should be the name of your client, e.g. Phone's name, PC's name, IP address. 
6. Sign out IE client from other browsers.
7. Refresh both browsers, IE client is signed out
8. Login again with IE browser
9. Click "sign out everywhere" button on home page
10. Refresh both browsers and both are signed out.

The sample defines a new property "Clients" in ApplicationUser, which persists client information in database. 
The client data will be saved when user sign in and removed when user sign out.
The client id will added to user identity's claims, which will be persisted in the cookie.
Whenever the request come in, the sample code will validate the cookie identity and check if 
the client id is existing in the database. If it's not, the request will be rejected by cookie auth.
The specific to register the validation is in Startup.Auth.cs:

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = ApplicationCookieIdentityValidator.OnValidateIdentity(
                        validateInterval: TimeSpan.FromMinutes(0),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

When user click sign out specific client button, the client is removed from database.
When user click sign out everywhere button, all the clients of the user are removed from database.

