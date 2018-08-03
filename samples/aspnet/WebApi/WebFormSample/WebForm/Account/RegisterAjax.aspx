<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterAjax.aspx.cs" Inherits="WebForm.Account.RegisterAjax" %>

<hgroup class="title">
    <h1>Register.</h1>
    <h2>Use the form below to create a new account.</h2>
</hgroup>
<form action="<%: GetHandlerUrl() %>" method="post">
    <p class="message-info">
        Passwords are required to be a minimum of <%: Membership.MinRequiredPasswordLength %> characters in length.
    </p>

    <fieldset>
        <legend>Registration Form</legend>
        <ol>
            <li>
                <label for="UserName">User name</label>
                <input name="UserName" id="UserName" type="text" value="" />
            </li>
            <li>
                <label for="Email">Email Address</label>
                <input name="Email" id="Email" type="text" value="" />
            </li>
            <li>
                <label for="Password">Password</label>
                <input name="Password" id="Password" type="password" value="" />
            </li>
            <li>
                <label for="ConfirmPassword">Confirm password</label>
                <input name="ConfirmPassword" id="ConfirmPassword" type="password" value="" />
            </li>
        </ol>
        <input type="submit" value="Register" />
    </fieldset>
</form>