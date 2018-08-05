using Account.Client;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Client;
using Windows.Security.Credentials;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Todo.WindowsStore
{
    public class AccountSettings
    {
        AppSettings settings = new AppSettings();
        string username;
        string localProvider;

        Frame Frame
        {
            get
            {
                return (Frame)Window.Current.Content;
            }
        }

        public void CommandsRequested(SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs eventArgs)
        {
            eventArgs.Request.ApplicationCommands.Add(SettingsCommand.AccountsCommand);
        }

        public async void AccountCommandsRequested(AccountsSettingsPane accountsSettingsPane, AccountsSettingsPaneCommandsRequestedEventArgs eventArgs)
        {
            AccountsSettingsPaneEventDeferral deferral = eventArgs.GetDeferral();

            HttpResult<ManageInfo> result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.GetManageInfoAsync();
            }

            if (!result.Succeeded)
            {
                await ErrorDialog.ShowErrorsAsync(result.Errors);
                // The log off command is not available on the account settings page if there are no accounts, so log off now
                LogOff();
                deferral.Complete();
                return;
            }

            ManageInfo manageInfo = result.Content;
            this.username = manageInfo.UserName;
            this.localProvider = manageInfo.LocalLoginProvider;

            eventArgs.HeaderText = "Manage your account logins";

            ////Add WebAccountProviders
            Dictionary<string, WebAccountProvider> webProviders = new Dictionary<string, WebAccountProvider>();
            WebAccountProviderCommandInvokedHandler providerCommandHandler = new WebAccountProviderCommandInvokedHandler(WebAccountProviderInvokedHandler);
            foreach (ExternalLogin externalLogin in manageInfo.ExternalLoginProviders)
            {
                WebAccountProvider provider = new WebAccountProvider(externalLogin.Url, externalLogin.Name, App.LoginIcons[externalLogin.Name]);
                WebAccountProviderCommand providerCommand = new WebAccountProviderCommand(provider, providerCommandHandler);
                eventArgs.WebAccountProviderCommands.Add(providerCommand);
                webProviders[provider.DisplayName] = provider;
            }

            WebAccountProvider localLoginProvider = new WebAccountProvider(manageInfo.LocalLoginProvider, manageInfo.LocalLoginProvider, null);
            webProviders[manageInfo.LocalLoginProvider] = localLoginProvider;

            ////Add WebAccounts and local accounts if available.
            bool hasLocalLogin = false;
            foreach (UserLoginInfo userLoginInfo in manageInfo.Logins)
            {
                WebAccountCommandInvokedHandler accountCommandHandler;
                SupportedWebAccountActions supportedActions = SupportedWebAccountActions.None;
                if (manageInfo.Logins.Length > 1)
                {
                    supportedActions |= SupportedWebAccountActions.Remove;
                }
                if (userLoginInfo.LoginProvider == manageInfo.LocalLoginProvider)
                {
                    hasLocalLogin = true;
                    supportedActions |= SupportedWebAccountActions.Manage;
                    accountCommandHandler = new WebAccountCommandInvokedHandler(LocalWebAccountInvokedHandler);
                }
                else
                {
                    accountCommandHandler = new WebAccountCommandInvokedHandler(WebAccountInvokedHandler);
                }
                WebAccount webAccount = new WebAccount(webProviders[userLoginInfo.LoginProvider], userLoginInfo.ProviderKey, WebAccountState.Connected);

                WebAccountCommand webAccountCommand = new WebAccountCommand(webAccount, accountCommandHandler, supportedActions);
                eventArgs.WebAccountCommands.Add(webAccountCommand);
            }

            if (!hasLocalLogin)
            {
                WebAccountProviderCommandInvokedHandler localProviderCmdHandler = new WebAccountProviderCommandInvokedHandler(LocalProviderInvokedHandler);
                WebAccountProviderCommand localProviderCommand = new WebAccountProviderCommand(localLoginProvider, localProviderCmdHandler);
                eventArgs.WebAccountProviderCommands.Add(localProviderCommand);
            }

            SettingsCommand logOffCommand = new SettingsCommand("Logoff", "Log off", new UICommandInvokedHandler(LogOffHandler));
            eventArgs.Commands.Add(logOffCommand);

            deferral.Complete();
        }

        private void LocalProviderInvokedHandler(WebAccountProviderCommand command)
        {
            SetPasswordFlyout setPasswordFlyout = new SetPasswordFlyout(username);
            setPasswordFlyout.Show();
        }

        private async void WebAccountProviderInvokedHandler(WebAccountProviderCommand command)
        {
            string externalLoginUri = command.WebAccountProvider.Id;
            ExternalLoginResult loginExternalResult = await ExternalLoginManager.GetExternalAccessTokenAsync(externalLoginUri);

            string accessToken = loginExternalResult.AccessToken;
            if (accessToken != null)
            {
                HttpResult result;
                using (AccountClient accountClient = ClientFactory.CreateAccountClient())
                {
                    result = await accountClient.AddExternalLoginAsync(new AddExternalLogin() { ExternalAccessToken = accessToken });
                }
                if (result.Succeeded)
                {
                    AccountsSettingsPane.Show();
                }
                else
                {
                    await ErrorDialog.ShowErrorsAsync(result.Errors);
                }
            }
            else
            {
                await ErrorDialog.ShowErrorAsync("Failed to connect to external account.");
            }
        }

        private async Task WebAccountInvokedHandlerCoreAsync(WebAccountCommand command, WebAccountInvokedArgs eventArgs)
        {
            if (eventArgs.Action == WebAccountAction.Remove)
            {
                string loginProvider = command.WebAccount.WebAccountProvider.DisplayName;
                string providerKey = command.WebAccount.UserName;

                RemoveLogin removeLogin = new RemoveLogin()
                {
                    LoginProvider = loginProvider,
                    ProviderKey = providerKey
                };

                HttpResult result;
                using (AccountClient accountClient = ClientFactory.CreateAccountClient())
                {
                    result = await accountClient.RemoveLoginAsync(removeLogin);
                }

                if (result.Succeeded)
                {
                    if (loginProvider == localProvider)
                    {
                        settings.ClearPasswordCredentials();
                    }
                }
                else
                {
                    await ErrorDialog.ShowErrorsAsync(result.Errors);
                }
            }
        }

        private async void WebAccountInvokedHandler(WebAccountCommand command, WebAccountInvokedArgs eventArgs)
        {
            await WebAccountInvokedHandlerCoreAsync(command, eventArgs);
        }

        private async void LocalWebAccountInvokedHandler(WebAccountCommand command, WebAccountInvokedArgs eventArgs)
        {
            await WebAccountInvokedHandlerCoreAsync(command, eventArgs);
            if (eventArgs.Action == WebAccountAction.Manage)
            {
                ChangePasswordFlyout changePasswordFlyout = new ChangePasswordFlyout(this.username);
                changePasswordFlyout.Show();
            }
        }

        private void LogOffHandler(IUICommand command)
        {
            LogOff();
        }

        private void LogOff()
        {
            settings.LogOff();
            Frame.Navigate(typeof(TodoPage));
        }
    }
}
