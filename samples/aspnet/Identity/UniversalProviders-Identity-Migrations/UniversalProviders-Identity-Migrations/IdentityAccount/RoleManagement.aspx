<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleManagement.aspx.cs" Inherits="UniversalProviders_Identity_Migrations.IdentityAccount.RoleManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3>Create Roles
        </h3>
        <asp:ValidationSummary runat="server" />    
        <div>
            <asp:Label Text="Role name" runat="server" />
            <asp:TextBox runat="server" ID="AddRole" />
            <asp:Button Text="Add Role" ID="AddRoleButton" OnClick="AddRoleButton_Click" runat="server" />
        </div>
        <div>
            <h3>Add user to role
            </h3>
            <div>
                <asp:Label Text="Username" runat="server" />
                <asp:TextBox runat="server" ID="Username" />
            </div>
            <div>
                <asp:Label Text="Role Name" runat="server" />
                <asp:TextBox runat="server" ID="Rolename" />
            </div>
            <asp:Button Text="Add User to Role" ID="AddUserRole" OnClick="AddUserRole_Click" runat="server" />
        </div>
    </div>
</asp:Content>
