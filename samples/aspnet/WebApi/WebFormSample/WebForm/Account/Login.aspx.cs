using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebForm.Account
{
    public partial class Login : Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Request.QueryString["content"]))
            {
                Response.Redirect("~/Account/LoginAjax.aspx");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }
    }
}