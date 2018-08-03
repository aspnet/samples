<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfileManagement.aspx.cs" Inherits="UniversalProviders_Identity_Migrations.Account.ProfileManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <h3>Profile Management for user: <%= User.Identity.Name %></h3>
    <asp:FormView runat="server" ItemType="UniversalProviders_Identity_Migrations.Models.ProfileInfo" ID="ProfileForm"
        InsertMethod="ProfileForm_InsertItem" UpdateMethod="ProfileForm_UpdateItem" SelectMethod="ProfileForm_GetItem"
        DefaultMode="Edit">
        <InsertItemTemplate>
            <h4>Insert profile</h4>
            <div>
                Date of Birth:
        <asp:TextBox runat="server" ID="DateOfBirth" Text='<%# BindItem.DateOfBirth %>' />
            </div>
            <div>
                Weight:
        <asp:TextBox runat="server" ID="Weight" Text='<%# BindItem.UserStats.Weight %>' />
            </div>
            <div>
                Height:
        <asp:TextBox runat="server" ID="Height" Text='<%# BindItem.UserStats.Height %>' />
            </div>
            <div>
                City:
        <asp:TextBox runat="server" ID="City" Text='<%# BindItem.City %>' />
            </div>
            <div>
                <asp:Button Text="Submit" CommandName="Insert" ID="AddProfile" runat="server" />
            </div>
        </InsertItemTemplate>
        <EditItemTemplate>
            <h4>Edit profile</h4>
            <div>
                Date of Birth:
        <asp:TextBox runat="server" ID="DateOfBirth" Text='<%# BindItem.DateOfBirth %>' />
            </div>
            <div>
                Weight:
        <asp:TextBox runat="server" ID="Weight" Text='<%# BindItem.UserStats.Weight %>' />
            </div>
            <div>
                Height:
        <asp:TextBox runat="server" ID="Height" Text='<%# BindItem.UserStats.Height %>' />
            </div>
            <div>
                City:
        <asp:TextBox runat="server" ID="City" Text='<%# BindItem.City %>' />
            </div>
            <div>
                <asp:Button Text="Submit" CommandName="Update" ID="AddProfile" runat="server" />
            </div>
        </EditItemTemplate>
    </asp:FormView>
</asp:Content>
