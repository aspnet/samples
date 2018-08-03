<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginAjax.aspx.cs" Inherits="WebForm.Account.LoginAjax" %>

<hgroup class="title">
    <h1>Log in.</h1>
    <h2>Enter your user name and password below.</h2>
</hgroup>
<form method="post" action="<%: GetHandlerUrl() %>">
    <fieldset>
        <legend>Log in Form</legend>
        <ol>
            <li>
                <label for="UserName">User name</label>
                <input name="UserName" id="UserName" type="text" value="" />
            </li>
            <li>
                <label for="Password">Password</label>
                <input name="Password" id="Password" type="password" value="" />
            </li>
            <li>
                <input name="RememberMe" id="RememberMe" type="checkbox" />
                <label for="RememberMe">Remember me</label>
            </li>
        </ol>
        <input type="submit" value="Log in" />
    </fieldset>
</form>
<p>
    <a href="<%: GetRegisterUrl() %>">Register</a>
    if you don't have an account.
</p>
