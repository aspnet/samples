using Account.Client;
using Microsoft.Phone.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class AddAccountPage : PhoneApplicationPage
    {
        string localProvider;

        public AddAccountPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.AddAccountItemControl.Items.Clear();

            HttpResult<ManageInfo> result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.GetManageInfoAsync();
            }
            
            if (result.Succeeded)
            {
                ManageInfo manageInfo = result.Content;
                foreach (ExternalLogin externalLogin in manageInfo.ExternalLoginProviders)
                {
                    string provider = externalLogin.Name;
                    Account account = new Account()
                    {
                        Provider = externalLogin.Name,
                        Icon = Account.GetAccountIcon(provider),
                        ProviderUri = externalLogin.Url
                    };
                    AddAccountItemControl.Items.Add(account);
                }

                localProvider = manageInfo.LocalLoginProvider;
                UserLoginInfo localLogin = manageInfo.Logins.FirstOrDefault(login => login.LoginProvider == localProvider);
                bool hasLocalLogin = localLogin != null;
                if (!hasLocalLogin)
                {
                    Account localAccount = new Account()
                    {
                        Provider = localProvider,
                        Icon = Account.GetAccountIcon(localProvider)
                    };
                    AddAccountItemControl.Items.Add(localAccount);
                }
            }
            else
            {
                ErrorDialog.ShowErrors(result.Errors);
                this.NavigationService.GoBack();
            }
        }

        private async void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Account account = (Account)((FrameworkElement)sender).DataContext;
            if (account.Provider == localProvider)
            {
                this.NavigationService.Navigate(new Uri("/SetPasswordPage.xaml", UriKind.Relative));
            }
            else
            {
                ExternalLoginResult loginExternalResult = await ExternalLoginManager.GetExternalAccessTokenAsync(account.ProviderUri);

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
                        this.NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        ErrorDialog.ShowErrors(result.Errors);
                    }
                }
            }
        }
    }
}