Use the sampe in scenario:
You want to migrate an existing MVC4 project from simple memebership to identity API, 
AND you don't want to change database scheme, 
AND you don't want to change authenticmation layer to OWIN middlewares.

Follow the following steps to apply the migration:
1. Install package Microsoft.AspNet.Identity.EntityFramework equals or later than version 2.0.0-alpha1 in your MVC4 project
2. Copy Models/AccountModel.cs, Models/IdentityModels.cs and Models/SimpleMembershipDbContext.cs to your project folder.
3. If you didn't change AccountController in your MVC4 project after it's created from template, 
simply copy Controllers/AccountController.cs to replace your existing AccountController.cs
4. Copy all the view files under Views/Account/*.cshtml to corresponding folder in MVC4 project
5. Comment out InitializeSimpleMembershipAttribute related code in MVC4 project.

The migrated project will still use the database created by simple membership but developer can use identity API to manage it.
It also uses existing FormAuthenticationModule instead of OWIN middleware. 