<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="WebForm.Account.ChangePassword" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Page.Title %>.</h1>
        <h2>Use the form below to change your password.</h2>
    </hgroup>

    <p class="message-info">
        New passwords are required to be a minimum of <%: Membership.MinRequiredPasswordLength %> characters in length.
    </p>

    <asp:ChangePassword runat="server" ID="ChangeUserPassword" CancelDestinationPageUrl="~/" EnableViewState="false" RenderOuterTable="false" SuccessPageUrl="ChangePasswordSuccess.aspx">
        <ChangePasswordTemplate>
            <p class="validation-summary-errors">
                <asp:Literal runat="server" ID="FailureText" />
            </p>
            <fieldset class="changePassword">
                <legend>Account Information</legend>
                <ol>
                    <li>
                        <asp:Label runat="server" ID="CurrentPasswordLabel" AssociatedControlID="CurrentPassword">Current password</asp:Label>
                        <asp:TextBox runat="server" ID="CurrentPassword" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                                CssClass="field-validation-error" ErrorMessage="The current password field is required." />
                    </li>
                    <li>
                        <asp:Label runat="server" ID="NewPasswordLabel" AssociatedControlID="NewPassword">New password</asp:Label>
                        <asp:TextBox runat="server" ID="NewPassword" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                                CssClass="field-validation-error" ErrorMessage="The new password is required." />
                    </li>
                    <li>
                        <asp:Label runat="server" ID="ConfirmNewPasswordLabel" AssociatedControlID="ConfirmNewPassword">Confirm new password</asp:Label>
                        <asp:TextBox runat="server" ID="ConfirmNewPassword" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ConfirmNewPassword"
                                CssClass="field-validation-error" Display="Dynamic" ErrorMessage="Confirm new password is required." />
                        <asp:CompareValidator runat="server" ControlToCompare="NewPassword" ControlToValidate="ConfirmNewPassword"
                                CssClass="field-validation-error" Display="Dynamic" ErrorMessage="The new password and confirmation password do not match." />
                    </li>
                </ol>
                <asp:Button runat="server" CommandName="ChangePassword" Text="Change password" />
            </fieldset>
        </ChangePasswordTemplate>
    </asp:ChangePassword>
</asp:Content>
