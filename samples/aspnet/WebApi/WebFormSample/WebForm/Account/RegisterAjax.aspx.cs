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
    public partial class RegisterAjax : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (String.Equals(Request.HttpMethod, "POST", StringComparison.Ordinal))
            {
                HandleAjaxRequest();
            }
        }

        protected string GetHandlerUrl()
        {
            return ResolveUrl("RegisterAjax.aspx?ReturnUrl=" + GetReturnUrl());
        }

        private void HandleAjaxRequest()
        {
            var errors = new List<string>();

            var username = Request.Form["UserName"];
            var password = Request.Form["Password"];
            var confirmPassword = Request.Form["ConfirmPassword"];
            var email = Request.Form["Email"];

            Response.ContentType = "application/json";

            AccountHelpers.Require(errors, username, "The User name field is required");
            AccountHelpers.Require(errors, password, "The Password field is required");
            AccountHelpers.Require(errors, confirmPassword, "The Confirm password field is required");
            AccountHelpers.Require(errors, email, "The Email address field is required");
            if (!String.Equals(password, confirmPassword, StringComparison.Ordinal))
            {
                errors.Add("The Password and Confirmation password do not match.");
            }

            if (errors.Count == 0)
            {
                MembershipCreateStatus status;
                Membership.CreateUser(username, password, email, passwordQuestion: null, passwordAnswer: null, isApproved: true, status: out status);

                if (status == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(username, createPersistentCookie: false);
                }
                else
                {
                    errors.Add(ErrorCodeToString(status));
                }
            }

            AccountHelpers.WriteJsonResponse(Response, errors);
        }

        private string GetReturnUrl()
        {
            return HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}