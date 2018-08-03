using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UniversalProviders_Identity_Migrations.IdentityModels;
using Microsoft.AspNet.Identity;

namespace UniversalProviders_Identity_Migrations.Account
{
    public partial class ProfileManagement : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void ProfileForm_InsertItem()
        {
            //var item = profile.ProfileInfo;

            //TryUpdateModel(item);

            //if (ModelState.IsValid)
            //{
            //    profile.Save();
            //}
        }

        // The id parameter name should match the DataKeyNames value set on the control
        public void ProfileForm_UpdateItem(int? id)
        {
            var manager = new UserManager();

            var user = manager.FindByName(User.Identity.Name);

            var item = user.Profile;

            // Load the item here, e.g. item = MyDataLayer.Find(id);
            if (item == null)
            {
                // The item wasn't found
                ModelState.AddModelError("", String.Format("Item with id {0} was not found", id));
                return;
            }
            TryUpdateModel(item);
            if (ModelState.IsValid)
            {
                manager.Update(user);
                // Save changes here, e.g. MyDataLayer.SaveChanges();
            }
        }

        // The id parameter should match the DataKeyNames value set on the control
        // or be decorated with a value provider attribute, e.g. [QueryString]int id
        public UniversalProviders_Identity_Migrations.Models.ProfileInfo ProfileForm_GetItem(int? id)
        {
            var manager = new UserManager();

            var user = manager.FindByName(User.Identity.Name);

            return user.Profile;
        }
    }
}