using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using CustomMembershipSample.IdentityModels;

namespace CustomMembershipSample.Controllers
{
    public class IdentityAccountController : Controller
    {
        //
        // GET: /IdentityAccount/Login

        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /IdentityAccount/Login

        [HttpPost]
        public ActionResult Login(CustomMembershipSample.Models.LogOnModel model, string returnUrl)
        {
            var manager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            var user = manager.Find(model.UserName, model.Password);

            if (user != null)
            {
                // Set auth cookie for authenticated user
                HttpContext.GetOwinContext().Authentication.SignIn(manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
                return RedirectToAction("Index", "Home");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(CustomMembershipSample.Models.RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                var manager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var user = new CustomMembershipSample.IdentityModels.AppUser() { Username = model.UserName, Email = model.Email };
                user.Addresses = new List<Address>();
                user.Addresses.Add(new Address() { City = model.City, State = model.State, Country = model.Country });

                // Create user with supplied credentials
                var result = manager.Create<CustomMembershipSample.IdentityModels.AppUser, int>(user, model.Password);

                if (result == IdentityResult.Success)
                {
                    // On success set the sign in cookie
                    HttpContext.GetOwinContext().Authentication.SignIn(manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie));
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.FirstOrDefault());
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            // Remove signin cookie
            HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}
