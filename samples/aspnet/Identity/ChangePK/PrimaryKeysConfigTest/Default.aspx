<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PrimaryKeysConfigTest._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>ASP.NET Identity sample app</h1>
        <p class="lead">Web application to demonstrate changing primary type in Identity from string to int</p>
    </div>

    <div class="row">
        <div class="col-md-4">
            <h2>To change the primary key type</h2>
            <p>
                Check the IdentityModel.cs file. All the Identity model classes are extended to have primary key type int for user and role. This is then hooked on user and role store which is then used in the Manager classes
            </p>
            <p>
                The Identity API used in files in the 'Account' folder are then edited to include the primary key type. For example, manager.Find&lt;ApplicationUser,int&gt;
            </p>
            <p>
                The key type change be changed to GUID by replacing the int used in model classes and the APIs
            </p>
        </div>
        <div class="col-md-4">
            <h2>Account confirmation and password reset</h2>
            <p>
                The Accounts folder has new files for user confirmation and reset password. In the current sample, registering a user, confirms them by default
            </p>
            <p>
                User can reset password from login page. The email of the user needs to be supplied which redirects to the reset password form.
            </p>
            <h3>Security note</h3>
            <p>
                The method to generate user confirmation url and reset password url shown in the sample is for demo purposes only. This is insecure and should not be replicated in real world apps.
                The generated urls must be sent to the user via email as a link and then the user should access their emails for confirmation/reset password.
            </p>
        </div>
        <div class="col-md-4">
            <h2>Delete users and roles</h2>
            <p>
                The new delete API can be used to delete existing roles and users. The sample page is Account/DeleteTest
            </p>
            <p>
                Enter the name of existing user or role and call delete. The admin role is created at the start of application in the Global.asax file
            </p>
        </div>
    </div>
</asp:Content>
