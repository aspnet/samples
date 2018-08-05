using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Moq;
using Microsoft.Owin.Security;
using UnitTestAccountController.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Security.Claims;
using UnitTestAccountController.Controllers;
using System.Web.Mvc;
using Microsoft.Owin;
using System.Linq;

namespace UnitTestAccountController.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void TestSuccessfulLogin()
        {
            // Arrange
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object);
            var loginModel = new LoginViewModel { 
                UserName = "a",
                Password = "b",
                RememberMe = false
            };
            var returnUrl = "/foo";
            var user = new ApplicationUser
            {
                UserName = loginModel.UserName
            };
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            userManager.Setup(um => um.FindAsync(loginModel.UserName, loginModel.Password)).Returns(Task.FromResult(user));
            userManager.Setup(um => um.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)).Returns(Task.FromResult(identity));

            var controller = new AccountController(userManager.Object);
            var helper = new MvcMockHelper(controller);

            // Act
            var actionResult = controller.Login(loginModel, returnUrl).Result;

            // Assert
            var redirectResult = actionResult as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(returnUrl, redirectResult.Url);

            Assert.AreEqual(loginModel.UserName, helper.OwinContext.Authentication.AuthenticationResponseGrant.Identity.Name);
            Assert.AreEqual(DefaultAuthenticationTypes.ExternalCookie, helper.OwinContext.Authentication.AuthenticationResponseRevoke.AuthenticationTypes.First());
        }

        [TestMethod]
        public void TestUnsuccessfulLogin()
        {
            // Arrange
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object);
            var loginModel = new LoginViewModel
            {
                UserName = "a",
                Password = "b",
                RememberMe = false
            };
            var returnUrl = "/foo";
            var user = new ApplicationUser
            {
                UserName = loginModel.UserName
            };
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

            userManager.Setup(um => um.FindAsync(loginModel.UserName, loginModel.Password)).Returns(Task.FromResult<ApplicationUser>(null));

            var controller = new AccountController(userManager.Object);
            var helper = new MvcMockHelper(controller);

            // Act
            var actionResult = controller.Login(loginModel, returnUrl).Result;

            // Assert
            Assert.IsTrue(actionResult is ViewResult);
            var errors = controller.ModelState.Values.First().Errors;
            Assert.AreEqual(1, errors.Count());
        }

        [TestMethod]
        public void TestSuccessfulRegister()
        {
            // Arrange
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object);
            var registerModel = new RegisterViewModel
            {
                UserName = "a",
                Password = "b",
                ConfirmPassword = "b"
            };
            var result = IdentityResult.Success;
            var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            identity.AddClaim(new Claim(ClaimTypes.Name, registerModel.UserName));

            userManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), registerModel.Password)).Returns(Task.FromResult(result));
            userManager.Setup(um => um.CreateIdentityAsync(It.IsAny<ApplicationUser>(), DefaultAuthenticationTypes.ApplicationCookie)).Returns(Task.FromResult(identity));

            var controller = new AccountController(userManager.Object);
            var helper = new MvcMockHelper(controller);

            // Act
            var actionResult = controller.Register(registerModel).Result;

            // Assert
            var redirectResult = actionResult as RedirectToRouteResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Home", redirectResult.RouteValues["controller"]);
            Assert.AreEqual("Index", redirectResult.RouteValues["action"]);

            Assert.AreEqual(registerModel.UserName, helper.OwinContext.Authentication.AuthenticationResponseGrant.Identity.Name);
            Assert.AreEqual(DefaultAuthenticationTypes.ExternalCookie, helper.OwinContext.Authentication.AuthenticationResponseRevoke.AuthenticationTypes.First());
        }
    }
}
