using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniversalProviders_Identity_Migrations.Models;

namespace UniversalProviders_Identity_Migrations.Account
{
    public partial class AddProfileData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Add_Click(object sender, EventArgs e)
        {
            AppProfile profile = AppProfile.GetProfile(User.Identity.Name);

            profile.ProfileInfo.DateOfBirth = DateTime.Parse(DateOfBirth.Text);

            profile.ProfileInfo.UserStats.Weight = Int32.Parse(Weight.Text);
            profile.ProfileInfo.UserStats.Height = Int32.Parse(Height.Text);

            profile.ProfileInfo.City = City.Text;

            profile.Save();
        }
    }
}