<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="UniversalProviders_Identity_Migrations.IdentityAcccount.Register" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Register a user</h2>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
    <fieldset>
        <div>
            <asp:Label ID="Label1" Text="Username" AssociatedControlID="Username" runat="server" />
            <asp:TextBox runat="server" ID="Username" />
            <asp:RequiredFieldValidator ErrorMessage="Username is required" ControlToValidate="Username" runat="server" />
        </div>
        <div>
            <asp:Label ID="Label2" Text="Password" AssociatedControlID="Password" runat="server" />
            <asp:TextBox runat="server" ID="Password" TextMode="Password" />
            <asp:RequiredFieldValidator ErrorMessage="Password is required" ControlToValidate="Password" runat="server" />
        </div>
        <div>
            <asp:Label ID="Label3" Text="ConfirmPassword" AssociatedControlID="ConfirmPassword" runat="server" />
            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" />
            <asp:RequiredFieldValidator ErrorMessage="Confirm password is required" ControlToValidate="ConfirmPassword" runat="server" />
            <asp:CompareValidator ErrorMessage="Password and confirm password" ControlToValidate="ConfirmPassword" ControlToCompare="Password" runat="server" />
        </div>
        <div>
            <asp:Button Text="Register" ID="Submit" OnClick="Submit_Click" runat="server" />
        </div>
    </fieldset>
</asp:Content>

