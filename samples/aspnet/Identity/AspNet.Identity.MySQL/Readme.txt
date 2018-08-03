Sample project for the article http://www.asp.net/identity/overview/extensibility/implementing-a-custom-mysql-aspnet-identity-storage-provider 


This is an example to implement a MYSQL store for ASP.NET Identity updated to use ASP.NET Identity 2.0

Steps to run project

- Clone repo and open project in VS with Update 2 installed
- Build project to restore packages and build project
- In the solution, add a new one ASP.NET project with MVC and Individual Authentication
- Uninstall Microsoft.AspNet.Identity.EntityFramework package from the web application
- Update connection string to use the MySQL database as needed
- In the IdentityModel.cs, let ApplicationUser class extend from Identity user in AspNet.Identity.MySQL
- ApplicationDbContext extend from MySqlDatabase and the contructor take a single parameter with the connectionstring name
- In the ApplicationManager.Create method, replace instantiating UserManager as shown below

	var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>() as MySQLDatabase));

- If any properties are added to the ApplicationUser class, then update the Insert, GetUserByName, GetUserById and Update methods in AspNet.Identity.MySQL project