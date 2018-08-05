**** Custom simple membership application ****

This is a sample application that demonstrates hooking in ASP.NET Identity in an existing application that used a custom membership provider implementation for user and role management.

1. Changeset: base app for custom membership provider + with db
The application has a model for users that is created using database first approach. We first create the user table with the columns to hold the desired user information. For sake of simplicity the model used in this sample is not too complex. 
- Create a database AppDb.mdf in the App_data folder of the application
- Create a AppUsers and Addresses table to hold the user and address information
- Add a ADO.NET model in the application created from the above database

Now we create custom simple membership provider that creates users and stores them in the AppUser table. We also have a custom implementation used for password hashing before storing it to the database. 
- Create CustomProvider that extends MembershipProvider and implement the necessary methods.
- Configure this membership provider in the web.config

Execution: Create a new user and log in

Result
- Registering and logging the user should use the CustomProvider and store the information in the AppUsers table


2. Changeset: With ASP.NET Identity configured
In the changeset the application is migrated to use ASP.NET Identity and OWIN. 
- The OWIN pipeline is configured to use cookiemiddleware
- The AppUser extends IdentityUser and rest of the Identity classes are defined
- The AppDbContext is configured to map to existing table
- Custom password hasher class is defined and plugged into Identity UserManager so that old user credentials are reused
- IdentityAccountController defined new Register and Login flow

Execution:
- Reuse old user credentials created in original membership.
- Create new user

Result: Old user logged in successfully. New user registered as expected