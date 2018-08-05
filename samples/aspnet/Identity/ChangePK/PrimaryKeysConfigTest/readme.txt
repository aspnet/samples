ASP.NET Identity sample app
Web application to demonstrate changing primary type in Identity from string to int

- To change the primary key type
Check the IdentityModel.cs file. All the Identity model classes are extended to have primary key type int for user and role. This is then hooked on user and role store which is then used in the Manager classes 

The Identity API used in files in the 'Account' folder are then edited to include the primary key type. For example, manager.Find<ApplicationUser,int> 

The key type change be changed to GUID by replacing the int used in model classes and the APIs 

- Account Confirmation and User confirmation
The Accounts folder has new files for user confirmation and reset password. In the current sample, registering a user, confirms them by default 

User can reset password from login page. The email of the user needs to be supplied which redirects to the reset password form. 

- Security note
The method to generate user confirmation url and reset password url shown in the sample is for demo purposes only. This is insecure and should not be replicated in real world apps. The generated urls must be sent to the user via email as a link and then the user should access their emails for confirmation/reset password. 

- Delete users and roles
The new delete API can be used to delete exisitng roles and users. The sample page is Account/DeleteTest 

Enter the name of existing user or role and call delete. The admin role is created at the start of application in the Global.asax file 
 
--------------------------------------------------------------------------------
