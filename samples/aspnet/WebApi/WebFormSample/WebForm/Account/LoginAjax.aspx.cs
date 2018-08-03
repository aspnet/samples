using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Account
{
    public partial class LoginAjax : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (String.Equals(Request.HttpMethod, "POST", StringComparison.Ordinal))
            {
                HandleAjaxRequest();
            }
        }

        protected string GetRegisterUrl()
        {
            return ResolveUrl("Register.aspx?ReturnUrl=" + GetReturnUrl());
        }

        protected string GetHandlerUrl()
        {
            return ResolveUrl("LoginAjax.aspx?ReturnUrl=" + GetReturnUrl());
        }

        private void HandleAjaxRequest()
        {
            var errors = new List<string>();

            var username = Request.Form["UserName"];
            var password = Request.Form["Password"];
            var rememberMe = String.Equals(Request.Form["RememberMe"], "on");
            var returnUrl = Request.QueryString["ReturnUrl"];
            var redirect = VirtualPathUtility.ToAbsolute("~/");

            Response.ContentType = "application/json";

            AccountHelpers.Require(errors, username, "The User name field is required");
            AccountHelpers.Require(errors, password, "The Password field is required");

            if (errors.Count == 0)
            {
                if (Membership.ValidateUser(username, password))
                {
                    FormsAuthentication.SetAuthCookie(username, rememberMe);
                    if (IsLocalUrl(returnUrl))
                    {
                        redirect = returnUrl;
                    }
                }
                else
                {
                    errors.Add("The user name or password provided is incorrect.");
                }
            }

            AccountHelpers.WriteJsonResponse(Response, errors, redirect);
        }

        private string GetReturnUrl()
        {
            return HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        private bool IsLocalUrl(string url)
        {
            return !String.IsNullOrEmpty(url) &&
                ((url[0] == '/' && (url.Length == 1 || (url[1] != '/' && url[1] != '\\'))) ||
                (url.Length > 1 && url[0] == '~' && url[1] == '/'));
        }
    }
}