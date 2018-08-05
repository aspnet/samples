﻿ASP.NET Identity sample app
Unit test sample to demonstrate testing account controller by mocking identity API and interfaces in an MVC application.
The sample use Moq as the mock framework.

- Mock up MVC controller
Check the MvcMockHelper.cs file. All the context object are setup up for an MVC controller during initialization, 
including HttpContext, Request, Response, Session, Server and OwinContext. 

- Mock up UserManager<T>
Check the Controllers/AccountControllerTests.cs file. It contains 3 sample tests convering login and register. 
All the public methods in UserManager<T> are virtual, so you can easily mock them.
Use the following code to mock the UserManager<T>:

    var userStore = new Mock<IUserStore<ApplicationUser>>();
    var userManager = new Mock<UserManager<ApplicationUser>>(userStore.Object);
	userManager.Setup(um => um.FindAsync(loginModel.UserName, loginModel.Password)).Returns(Task.FromResult(user));
    userManager.Setup(um => um.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie)).Returns(Task.FromResult(identity));

- Verify OWIN authentication status
Check the Controllers/AccountControllerTests.cs file. 
* OwinContext.Authentication.AuthenticationResponseGrant has the information for sigin identity
* OwinContext.Authentication.AuthenticationResponseRevoke has the information for signout
* OwinContext.Authentication.AuthenticationResponseChallenge has the information for challenge