using Account.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.WindowsStore.Common;
using WebApi.Client;
using Windows.Security.Credentials;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Todo.WindowsStore
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private TaskCompletionSource<string> accessTokenSource;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public LoginPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += AccountCommandsRequested;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            this.accessTokenSource = (TaskCompletionSource<string>)e.NavigationParameter;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested -= AccountCommandsRequested;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();

            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            HttpResult<AccessTokenResponse> result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.LoginAsync(username, password);
            }
            if (result.Succeeded)
            {
                AppSettings settings = new AppSettings();
                settings.SavePasswordCredential(username, password);
                bool completed = accessTokenSource.TrySetResult(result.Content.AccessToken);
                this.Frame.GoBack();
            }
            else
            {
                DisplayErrors(result.Errors);
            }
        }

        void DisplayErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                this.ErrorList.Items.Add(error);
            }
        }

        void ClearErrors()
        {
            this.ErrorList.Items.Clear();
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(RegisterPage), accessTokenSource);
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            AccountsSettingsPane.Show();
        }

        private async void AccountCommandsRequested(AccountsSettingsPane accountsSettingsPane, AccountsSettingsPaneCommandsRequestedEventArgs eventArgs)
        {
            AccountsSettingsPaneEventDeferral deferral = eventArgs.GetDeferral();

            HttpResult<ExternalLogin[]> result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.GetExternalLoginsAsync();
            }

            if (result.Succeeded)
            {
                eventArgs.HeaderText = "Please select a login provider.";
                WebAccountProviderCommandInvokedHandler providerCmdHandler = new WebAccountProviderCommandInvokedHandler(WebAccountProviderInvokedHandler);
                foreach (ExternalLogin externalLogin in result.Content)
                {
                    WebAccountProvider provider = new WebAccountProvider(externalLogin.Url, externalLogin.Name, App.LoginIcons[externalLogin.Name]);
                    WebAccountProviderCommand providerCommand = new WebAccountProviderCommand(provider, providerCmdHandler);
                    eventArgs.WebAccountProviderCommands.Add(providerCommand);
                }
            }
            else
            {
                await ErrorDialog.ShowErrorsAsync("Error connecting to external accounts.", result.Errors);
            }

            deferral.Complete();
        }

        private async void WebAccountProviderInvokedHandler(WebAccountProviderCommand command)
        {
            string externalLoginUri = command.WebAccountProvider.Id;
            ExternalLoginResult loginExternalResult = await ExternalLoginManager.GetExternalAccessTokenAsync(externalLoginUri);
            if (loginExternalResult.AccessToken != null)
            {
                // Save the access token so we can check if the user has registered
                AppSettings settings = new AppSettings();
                settings.AccessToken = loginExternalResult.AccessToken;

                HttpResult<UserInfo> result;
                using (AccountClient accountClient = ClientFactory.CreateAccountClient())
                {
                    result = await accountClient.GetUserInfoAsync();
                }
                if (result.Succeeded)
                {
                    if (result.Content.HasRegistered)
                    {
                        accessTokenSource.TrySetResult(loginExternalResult.AccessToken);
                        this.Frame.GoBack();
                    }
                    else
                    {
                        RegisterExternalPageParameters parameters = new RegisterExternalPageParameters()
                        {
                            AccessTokenSource = accessTokenSource,
                            ExternalLoginUri = externalLoginUri,
                            UserInfo = result.Content
                        };
                        this.Frame.Navigate(typeof(RegisterExternalPage), parameters);
                    }
                }
                else
                {
                    await ErrorDialog.ShowErrorsAsync("Failed to connected to external account.", result.Errors);
                }
            }
            else
            {
                await ErrorDialog.ShowErrorAsync("Failed to connected to external account.");
            }
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }
}
