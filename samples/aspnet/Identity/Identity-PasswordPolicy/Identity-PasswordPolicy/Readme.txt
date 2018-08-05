This is a sample project that outlines creating custom password policy using ASP.NET. Complete explaination available in the following blog post http://blogs.msdn.com/b/webdev/archive/2014/01/06/implementing-custom-password-policy-using-asp-net-identity.aspx

Password policies implemented 
i. Change the default password length requirement is 10 characters.

ii. The password should contain one special character, one number, one uppercase and one lowercase character.

iii. The user cannot reuse any of the last 5 previous passwords.

This application has all the policies implemented. 
- The CustomPasswordValidator class under IdentityExtensions implements checking if the supplied password is of length 10 characters. It also uses a regular expression to verify if the password has one special character and one numerci character
- The IdentityModels.cs file contains the custom class to store password history per user. The password hash is stored when user is created and everytime the user changes/resets password. This history is used to check if user is not reusing the old passwords.
Currently we check if the user does not use the last 5 passwords. The UserStore and UserManager methods are overriden to do this check.  
