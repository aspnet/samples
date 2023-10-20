# ASP.net Self Contained Pages for Troubleshooting Windows Integrated Authentication issues on IIS

Configuring Windows Integrated Authentication can be a complex endeavor to configure correctly on an IIS web-server which is part of an Active Directory domain. Troubleshooting issues with credential authentication via Windows Integrated Authentication and delegation of credentials scenarios from front-end to back-end servers is often cumbersome. The two pages included in this sample are intended to allow the collection of diagnostic data with minimal intrusion and configuration overhead in production environments.

## Sample Contents

The sample contains two ASP.net pages build using Web-Forms. Using Web-Forms was preferrable since it allows the pages to be self contained - markup and code behind can be located in the same file, while also allowing the user to see the code and not requiring any sort of pre-compilation for the pages to be deployed.

The pages are:

**WhoAmI.aspx** - which exposes authentication information about the requests made to the page. This page currently can distinguish between the following authentication types:
 - Windows Integrated Authentication (using Kerberos or NTLM)
 - Basic Authentication
 - Client Certificate Mapping (authentication)
 - Anonymous authentication.

It is able to display the server variables associated with the incoming request on the IIS server (hence allowing the inspection of authentication related headers that the client may send). 

The page also displays information about the authenticated user and the groups that the user may belong to in Active Directory, as well as the account used to execute the code of the page.

**ScrapperTest.aspx** - meant for more advanced scenarios where credential delegation using Kerberos is required. Typical scenarios look like the following:

Client computer ----> Font End Server ----> Back End Server

A domain joined client will use Windows Integrated Authentication to authenticate to a front-end web-application running on a Windows Server on top of IIS. The front-end server will then impersonate the authenticated user and execute the code of the requested resource on the server using the user's account. If this resource needs to make a request to a backend server for further data, this request can be made with the identity of the authenticated user. To the backend server it appears that the user logging in on the client computer is authenticating directly.

The page is made to be used in conjunction with the whomai.aspx page described above. The whomai.aspx page is placed on the backend server to be accessed by requests issued by the frontend server.

The scrapperTest.aspx page is placed on the frontend server and is to be accessed from the client computer, via Windows Integrated Authentication.

The UI of the scrapperTest.aspx page allows the user to input a target url, to which the page will issue a GET request to from the server it is running on. If the user is authenticated, the 'Use Credentials' checkbox on the page's UI is active and the user can choose to try and have their credentials delegated along with the request to the backend server.

In a Kerberos credential delegation scenario, the page to be accessed is the whomai.aspx page on the backend server, providing all authentication related information. The HTML of this page is then displayed inside the textarea control of the scrapperTest.aspx page running on the frontend server. This allows the user to verify that:
- credential delegation to the backend server took place
- what account the backend resource was executed with
- the type of authentication (and possibility impersonation) used to access the backend resource.

## Deployment

The two pages are meant to be used as in-place drops into production applications running ASP.net on .Net Framework. Simply:
- copy the whomai.aspx and optionally the scrappertest.aspx files to the root of your site (or web-application that you wish to troubleshoot)
- access the url of the site and or web-application appending the /whomai.aspx or /scrappertest.aspx at the end of the address.

## Known Limitations

If the site has predefined catch all-routes, then requests to the /whoami.aspx and or /scrappertest.aspx may be intercepted by the routing mechanism and never reach the pages, disallowing access.

In this case, you may wish to create a separate web-application underneath the main website or web-application in IIS. To do so, simply:
- select the IIS website or web-application you are aiming to troubleshoot from the left hand side tree view
- right click the selected node, and select 'New Web Application' from the context menu that appears.
- make sure the newly created web-application runs on the same application pool as the website or web-application you are targeting.
