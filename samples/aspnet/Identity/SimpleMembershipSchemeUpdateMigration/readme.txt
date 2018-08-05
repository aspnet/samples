Use the current migration sample for the below scenario:
You want to migrate an existing MVC4 project with simple membership settings to use identity API,
AND you want to upgrade the database scheme to identity scheme. In addition you want to use the new OWIN cookie middleware for setting/reading cookies instead of FormsAuthentication.

To view demo, follow the below listed steps:

1. Create a new MVC4 application. Run the application and register new user.
2. Run SimpleMembershipToIdentityMigration.sql script on the simple membership database
3. Install the following packages
	- Microsoft.AspNet.Identity.EntityFramework 2.1.0 
	- Microsoft.AspNet.Identity.OWIN 2.1.0 
	- Microsoft.Owin.Host.SystemWeb 2.1.0
4. Copy Models/IdentityModels.cs to old MVC4 project Models folder
5. Copy Controllers/AccountController.cs to override old MVC4 project controller
6. Copy the Startup class to the old MVC4 project root
7. Copy the IdentityConfig.cs file under App_Start folder

After the migration, you should be able to use default identity EF models and UserManager API to manage your membership database. The old user credentials should work fine. The new users being registered should supply emails for username. Do visit http://asp.net/identity to add two factor authentication to your migrated website.