<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="UniversalProviders_Identity_Migrations.IdentityAccount.Manage" %>

<%@ Register Src="~/IdentityAccount/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Manager account</h3>
    <section id="passwordForm">
        <asp:PlaceHolder runat="server" ID="successMessage" Visible="false" ViewStateMode="Disabled">
            <p class="text-success"><%: SuccessMessage %></p>
        </asp:PlaceHolder>

        <asp:ValidationSummary runat="server" />    
        <p>You're logged in as <strong><%: User.Identity.Name %></strong>.</p>
        <asp:PlaceHolder runat="server" ID="setPassword" Visible="false">
            <p>
                You do not have a local password for this site. Add a local
                password so you can log in without an external login.
            </p>
            <fieldset>
                <legend>Set Password Form</legend>
                <ol>
                    <li>
                        <asp:Label ID="Label1" runat="server" AssociatedControlID="password">Password</asp:Label>
                        <asp:TextBox runat="server" ID="password" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="password"
                            CssClass="field-validation-error" ErrorMessage="The password field is required."
                            Display="Dynamic" ValidationGroup="SetPassword" />

                        <asp:ModelErrorMessage ID="ModelErrorMessage1" runat="server" ModelStateKey="NewPassword" AssociatedControlID="password"
                            CssClass="field-validation-error" SetFocusOnError="true" />

                    </li>
                    <li>
                        <asp:Label ID="Label2" runat="server" AssociatedControlID="confirmPassword">Confirm password</asp:Label>
                        <asp:TextBox runat="server" ID="confirmPassword" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="confirmPassword"
                            CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The confirm password field is required."
                            ValidationGroup="SetPassword" />
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="Password" ControlToValidate="confirmPassword"
                            CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The password and confirmation password do not match."
                            ValidationGroup="SetPassword" />
                    </li>
                </ol>
                <asp:Button ID="Button1" runat="server" Text="Set Password" ValidationGroup="SetPassword" OnClick="setPassword_Click" />
            </fieldset>
        </asp:PlaceHolder>

        <asp:PlaceHolder runat="server" ID="changePassword" Visible="false">
            <h3>Change password</h3>

            <p class="validation-summary-errors">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
            <fieldset class="changePassword">
                <legend>Change password details</legend>
                <ol>
                    <li>
                        <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword">Current password</asp:Label>
                        <asp:TextBox runat="server" ID="CurrentPassword" TextMode="Password" CssClass="passswordEntry" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="CurrentPassword"
                            CssClass="field-validation-error" ErrorMessage="The current password field is required."
                            ValidationGroup="ChangePassword" />
                    </li>
                    <li>
                        <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword">New password</asp:Label>
                        <asp:TextBox runat="server" ID="NewPassword" CssClass="passwordEntry" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="NewPassword"
                            CssClass="field-validation-error" ErrorMessage="The new password is required."
                            ValidationGroup="ChangePassword" />
                    </li>
                    <li>
                        <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword">Confirm new password</asp:Label>
                        <asp:TextBox runat="server" ID="ConfirmNewPassword" CssClass="passwordEntry" TextMode="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ConfirmNewPassword"
                            CssClass="field-validation-error" Display="Dynamic" ErrorMessage="Confirm new password is required."
                            ValidationGroup="ChangePassword" />
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                            CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match."
                            ValidationGroup="ChangePassword" />
                    </li>
                </ol>
                <asp:Button ID="Button2" runat="server" OnClick="ChangePassword_Click" Text="Change password" ValidationGroup="ChangePassword" />
            </fieldset>
        </asp:PlaceHolder>
    </section>

    <section id="externalLoginsForm">

        <asp:ListView ID="ListView1" runat="server"
            ItemType="Microsoft.AspNet.Identity.UserLoginInfo"
            SelectMethod="GetLogins" DeleteMethod="RemoveLogin" DataKeyNames="LoginProvider,ProviderKey">

            <LayoutTemplate>
                <h4>Registered Logins</h4>
                <table class="table">
                    <tbody>
                        <tr runat="server" id="itemPlaceholder"></tr>
                    </tbody>
                </table>

            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td><%#: Item.LoginProvider %></td>
                    <td>
                        <asp:Button ID="Button3" runat="server" Text="Remove" CommandName="Delete" CausesValidation="false"
                            ToolTip='<%# "Remove this " + Item.LoginProvider + " login from your account" %>'
                            Visible="<%# CanRemoveExternalLogins %>" CssClass="btn btn-default" />
                    </td>
                </tr>
            </ItemTemplate>
        </asp:ListView>

        <uc:OpenAuthProviders ID="OpenAuthProviders1" runat="server" ReturnUrl="~/IdentityAccount/Manage.aspx" />
    </section>
    <asp:HyperLink NavigateUrl="~/IdentityAccount/ProfileManagement.aspx" Text="Manage Profile" CssClass="btn btn-default" runat="server" />
</asp:Content>
