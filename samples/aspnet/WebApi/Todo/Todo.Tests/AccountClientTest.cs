using Account.Client;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Owin;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Todo.Tests.Helpers;
using Todo.Web;
using Todo.Web.Providers;
using WebApi.Client;

namespace Todo.Tests
{
    [TestClass]
    public class AccountClientTest
    {
        TestServer server;
        AccountClient client;
        OAuth2BearerTokenHandler oAuthHandler;
        string userName = "danroth27";
        string password = "abc123";
        string invalidUserName = "danroth28";
        string invalidPassword = "123";
        string provider = "Facebook";
        string externalToken = "MockExternalToken";
        IdentityUser user;
        List<Claim> claims;
        ClaimsIdentityFactory<IdentityUser> factory = new ClaimsIdentityFactory<IdentityUser>();
        
        [TestInitialize]
        public void Init()
        {
            user = new IdentityUser(userName);

            ClaimsIdentity identity = new ClaimsIdentity("ExternalBearer", factory.UserNameClaimType, factory.RoleClaimType);
            identity.AddClaim(new Claim(factory.UserIdClaimType, "FacebookID", ClaimValueTypes.String, provider));

            Mock<UserManager<IdentityUser>> mock =
                new Mock<UserManager<IdentityUser>>(new Mock<IUserStore<IdentityUser>>().Object);
            mock.Setup(mgr => mgr.CreateAsync(It.Is<IdentityUser>(u => u.UserName == userName), password))
                .Returns(Task.FromResult(IdentityResult.Success));
            mock.Setup(mgr => mgr.CreateAsync(It.Is<IdentityUser>(u => u.UserName == userName)))
                .Returns(Task.FromResult(IdentityResult.Success)); 
            mock.Setup(mgr => mgr.CreateAsync(It.Is<IdentityUser>(u => u.UserName == invalidUserName), password))
                .Returns(Task.FromResult(IdentityResult.Failed("User already exists")));
            mock.Setup(mgr => mgr.CreateAsync(It.Is<IdentityUser>(u => u.UserName == invalidUserName)))
                .Returns(Task.FromResult(IdentityResult.Failed("User already exists")));
            mock.Setup(mgr => mgr.FindAsync(userName, password)).Returns(Task.FromResult(user));
            mock.Setup(mgr => mgr.FindAsync(invalidUserName, password))
                .Returns(Task.FromResult<IdentityUser>(null));
            mock.Setup(mgr => mgr.FindAsync(userName, invalidPassword))
                .Returns(Task.FromResult<IdentityUser>(null));
            mock.Setup(mgr => mgr.CreateIdentityAsync(It.Is<IdentityUser>(u => u.UserName == userName), It.IsAny<string>()))
                .Returns((IdentityUser u, string authType) =>
                    Task.FromResult(new ClaimsIdentity(claims, authType, factory.UserNameClaimType, factory.RoleClaimType)));

            Startup.UserManagerFactory = () => mock.Object;
            Startup.OAuthOptions.Provider = new ApplicationOAuthProvider(Startup.PublicClientId, Startup.UserManagerFactory);
            Startup.OAuthOptions.AccessTokenProvider = new AuthenticationTokenProvider()
            {
                OnReceive = context =>
                {
                    if (context.Token == externalToken)
                    {
                        context.SetTicket(
                            new AuthenticationTicket(identity,
                            new AuthenticationProperties() { ExpiresUtc = DateTimeOffset.Now.AddDays(1) }));
                    }
                }
            };

            server = TestServer.Create(app => 
            {
                new Todo.Web.Startup().Configuration(app);
                HttpConfiguration config = new HttpConfiguration();
                WebApiConfig.Register(config);
                app.UseWebApi(config);
            });

            oAuthHandler = new OAuth2BearerTokenHandler(new InMemoryAccessTokenStore(), new NullAccessTokenProvider());
            DelegatingHandler testHandler = new NonDisposableRequestHandler();
            client = new AccountClient(HttpClientFactory.Create(server.Handler, oAuthHandler, testHandler));
            client.HttpClient.BaseAddress = new Uri("http://localhost/");

            claims = new List<Claim>()
            {
                new Claim(factory.UserIdClaimType, user.Id, "http://www.w3.org/2001/XMLSchema#string"),
                new Claim(factory.UserNameClaimType, userName, "http://www.w3.org/2001/XMLSchema#string"),
                new Claim(
                    "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", 
                    "ASP.NET Identity", 
                    "http://www.w3.org/2001/XMLSchema#string")
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            server.Dispose();
            client.Dispose();
        }

        [TestMethod]
        public async Task Register()
        {
            RegisterUser registerUser = new RegisterUser(userName, password);

            HttpResult<RegisterResponse> result = await client.RegisterAsync(registerUser);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public async Task RegisterPasswordMismatch()
        {
            RegisterUser registerUser = new RegisterUser(userName, password, invalidPassword);

            HttpResult<RegisterResponse> result = await client.RegisterAsync(registerUser);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            RegisterResponse response = result.Content;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Message);
            Assert.IsTrue(response.HasErrors);
            Assert.IsNotNull(response.ConfirmPasswordErrors);
            Assert.IsTrue(response.ConfirmPasswordErrors.Length > 0);
        }

        [TestMethod]
        public async Task RegisterBadPassword()
        {
            RegisterUser registerUser = new RegisterUser(userName, invalidPassword);

            HttpResult<RegisterResponse> result = await client.RegisterAsync(registerUser);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            RegisterResponse response = result.Content;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Message);
            Assert.IsNotNull(response.PasswordErrors);
            Assert.IsTrue(response.PasswordErrors.Length > 0);
        }

        [TestMethod]
        public async Task RegisterUserAlreadyExists()
        {
            RegisterUser registerUser = new RegisterUser(invalidUserName, password);

            HttpResult<RegisterResponse> result = await client.RegisterAsync(registerUser);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            RegisterResponse response = result.Content;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Message);
            Assert.IsNotNull(response.ErrorMessages);
            Assert.IsTrue(response.ErrorMessages.Length > 0);
        }

        [TestMethod]
        public async Task RegisterUserNameRequired()
        {
            RegisterUser registerUser = new RegisterUser(null, password);

            HttpResult<RegisterResponse> result = await client.RegisterAsync(registerUser);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Succeeded);
            RegisterResponse response = result.Content;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Message);
            Assert.IsNotNull(response.UserNameErrors);
            Assert.IsTrue(response.UserNameErrors.Length > 0);
        }

        [TestMethod]
        public async Task Login()
        {
            HttpResult<AccessTokenResponse> loginResult = await client.LoginAsync(userName, password);

            Assert.IsTrue(loginResult.Succeeded);
            Assert.AreEqual(loginResult.Errors.Count, 0);
            Assert.IsNotNull(loginResult.Content.AccessToken);

            oAuthHandler.TokenStore.AccessToken = loginResult.Content.AccessToken;
            HttpResult<UserInfo> userInfoResult = await client.GetUserInfoAsync();
            Assert.IsNotNull(userInfoResult);
            Assert.IsTrue(userInfoResult.Succeeded);
            Assert.AreEqual(userInfoResult.Content.UserName, userName);
        }

        [TestMethod]
        public async Task LoginBadUsername()
        {
            HttpResult<AccessTokenResponse> result = await client.LoginAsync(invalidUserName, password);

            Assert.IsFalse(result.Succeeded);
            AccessTokenResponse response = result.Content;
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(response.Error, "invalid_grant");
            Assert.IsNotNull(response.ErrorDescription);
            Assert.IsFalse(response.Succeeded);
        }

        [TestMethod]
        public async Task LoginBadPassword()
        {
            HttpResult<AccessTokenResponse> result = await client.LoginAsync(userName, invalidPassword);

            AccessTokenResponse response = result.Content;
            Assert.IsNotNull(response.Error);
            Assert.AreEqual(response.Error, "invalid_grant");
            Assert.IsNotNull(response.ErrorDescription);
            Assert.IsFalse(response.Succeeded);
        }

        [TestMethod]
        public async Task Unauthorized()
        {
            HttpResult<UserInfo> result = await client.GetUserInfoAsync();

            Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized);
        }

        [TestMethod]
        public async Task GetExternalLogins()
        {
            HttpResult<ExternalLogin[]> result = await client.GetExternalLoginsAsync("Todo.WindowsStore");

            Assert.IsNotNull(result.Content);
        }

        [TestMethod]
        public async Task RegisterExternal()
        {
            oAuthHandler.TokenStore.AccessToken = externalToken;
            RegisterExternalUser externalUser = new RegisterExternalUser() { UserName = userName };

            HttpResult result = await client.RegisterExternalAsync(externalUser);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Succeeded);
        }

        [TestMethod]
        public async Task RegisterExternalUserAlreadyExists()
        {
            oAuthHandler.TokenStore.AccessToken = externalToken;
            RegisterExternalUser externalUser = new RegisterExternalUser() { UserName = invalidUserName };

            HttpResult<RegisterExternalResponse> result = await client.RegisterExternalAsync(externalUser);

            RegisterExternalResponse response = result.Content;
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Message);
            Assert.IsNotNull(response.ErrorMessages);
            Assert.IsTrue(response.ErrorMessages.Length > 0);
        }
    }
}
