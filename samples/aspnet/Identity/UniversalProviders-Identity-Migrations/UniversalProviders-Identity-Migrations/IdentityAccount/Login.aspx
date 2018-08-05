<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="UniversalProviders_Identity_Migrations.IdentityAcccount.Login" %>

<%@ Register Src="~/IdentityAccount/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Login user</h2>
    <asp:ValidationSummary runat="server" />
    <fieldset>
        <div>
            <asp:Label Text="Username" AssociatedControlID="Username" runat="server" />
            <asp:TextBox runat="server" ID="Username" />
            <asp:RequiredFieldValidator ErrorMessage="Username is required" ControlToValidate="Username" runat="server" />
        </div>
        <div>
            <asp:Label ID="Label2" Text="Password" AssociatedControlID="Password" runat="server" />
            <asp:TextBox runat="server" ID="Password" TextMode="Password" />
            <asp:RequiredFieldValidator ErrorMessage="Password is required" ControlToValidate="Password" runat="server" />
        </div>
        <div>
            <asp:CheckBox ID="RememberMe" runat="server" CssClass="checkbox" />
            <asp:Label ID="Label1" runat="server" AssociatedControlID="RememberMe" CssClass="checkbox">Remember me?</asp:Label>
        </div>
        <div>
            <asp:Button Text="Login" ID="UserLogin" OnClick="UserLogin_Click" runat="server" />
        </div>
    </fieldset>

    <section>
        <p>
            <asp:HyperLink runat="server" ID="RegisterHyperLink" ViewStateMode="Disabled">Register</asp:HyperLink>
            if you don't have a local account.
        </p>
    </section>

    <section id="socialLoginForm">
        <h2>Use another service to log in.</h2>
        <uc:OpenAuthProviders runat="server" ID="OpenAuthLogin" />
    </section>

</asp:Content>