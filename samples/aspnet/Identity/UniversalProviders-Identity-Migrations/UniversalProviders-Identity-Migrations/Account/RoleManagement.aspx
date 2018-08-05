<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RoleManagement.aspx.cs" Inherits="UniversalProviders_Identity_Migrations.Account.RoleManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <h3>Add Role</h3>
        <asp:TextBox runat="server" ID="AddRole" />
        <asp:Button Text="Add Role" ID="AddRoleButton" OnClick="AddRoleButton_Click" runat="server" />
    </div>

    <div>
        <h3> Add user to Role</h3>
        <div>
            Username:<asp:TextBox runat="server" ID="Username"/>
        </div>
        <div>
            Role:<asp:TextBox runat="server" Id="RoleToAdd"/> 
        </div>
        <div>
            <asp:Button Text="Add User to Role" ID="AddUserToRole" OnClick="AddUserToRole_Click" runat="server" />
        </div>
    </div>
</asp:Content>
