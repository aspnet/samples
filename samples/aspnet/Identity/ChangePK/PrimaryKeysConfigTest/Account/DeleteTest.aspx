<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DeleteTest.aspx.cs" Inherits="PrimaryKeysConfigTest.Account.DeleteTest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>delete user</h2>
    <div>
        Username:<asp:TextBox runat="server" Id="username"/>
        <asp:Button Text="Delete user" ID="DeleteUser" OnClick="DeleteUser_Click" runat="server" />
    </div>

    <h2>Delete role</h2>
    <div>
        Role:
        <asp:TextBox runat="server" Id="Rolename"/>
        <asp:Button Text="Delete Role" ID="DeleteRole" OnClick="DeleteRole_Click" runat="server" />
    </div>
</asp:Content>
