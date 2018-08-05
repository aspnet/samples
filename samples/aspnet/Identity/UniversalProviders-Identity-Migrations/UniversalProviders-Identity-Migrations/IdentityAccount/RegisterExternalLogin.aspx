<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegisterExternalLogin.aspx.cs" Inherits="UniversalProviders_Identity_Migrations.IdentityAccount.RegisterExternalLogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <hgroup class="title">
        <h1>Register with your <%: ProviderDisplayName %> account</h1>
    </hgroup>
    </div><br />

    <asp:ValidationSummary runat="server" />

    <div>
        <asp:PlaceHolder runat="server" ID="userNameForm">
            <fieldset>
                <legend>Association Form</legend>
                <p>
                    You've authenticated with <strong><%: ProviderDisplayName %></strong> as
                <strong><%: ProviderUserName %></strong>. Please enter a user name below for the current site
                and click the Log in button.
                </p>
                <ul>
                    <li class="email">
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="Username">User name</asp:Label>
                        <asp:TextBox runat="server" ID="Username" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="Username"
                            Display="Dynamic" ErrorMessage="User name is required" ValidationGroup="NewUser" />
                    </li>
                </ul>
                <asp:Button ID="Button1" runat="server" Text="Log in" ValidationGroup="NewUser" OnClick="LogIn_Click" />
            </fieldset>
        </asp:PlaceHolder>
    </div>
</asp:Content>

