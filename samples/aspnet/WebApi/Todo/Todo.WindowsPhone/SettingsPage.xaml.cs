using Account.Client;
using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Navigation;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        string localProvider;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            AccountsItemControl.Items.Clear();

            HttpResult<ManageInfo> result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.GetManageInfoAsync();
            }
            
            if (result.Succeeded)
            {
                ManageInfo manageInfo = result.Content;
                localProvider = manageInfo.LocalLoginProvider;
                foreach (UserLoginInfo userLoginInfo in manageInfo.Logins)
                {
                    Account account = new Account();
                    account.Provider = userLoginInfo.LoginProvider;
                    account.ProviderKey = userLoginInfo.ProviderKey;
                    account.Icon = Account.GetAccountIcon(userLoginInfo.LoginProvider);
                    AccountsItemControl.Items.Add(account);
                }
            }
            else
            {
                ErrorDialog.ShowErrors(result.Errors);
            }
        }

        private void ManageAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Account account = (Account)((FrameworkElement)sender).DataContext;
            if (account.Provider.Equals(localProvider, StringComparison.OrdinalIgnoreCase))
            {
                this.NavigationService.Navigate(new Uri("/ChangePasswordPage.xaml", UriKind.Relative));
            }
            else
            {
                this.NavigationService.Navigate(AccountDetailsPage.GetNavigationUri(account));
            }

        }

        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AddAccountPage.xaml", UriKind.Relative));
        }

        private void LogOffButton_Click(object sender, RoutedEventArgs e)
        {
            AppSettings settings = new AppSettings();
            settings.AccessToken = null;
            this.NavigationService.Navigate(new Uri("/TodoPage.xaml", UriKind.Relative));
        }

        private async void DeleteAccountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (AccountsItemControl.Items.Count == 1)
            {
                MessageBox.Show("You must have at least one account to login");
            }
            else {
                Account account = (Account)((MenuItem)sender).DataContext;
                RemoveLogin removeLogin = new RemoveLogin()
                {
                    LoginProvider = account.Provider,
                    ProviderKey = account.ProviderKey,
                };

                HttpResult result;
                using (AccountClient accountClient = ClientFactory.CreateAccountClient())
                {
                    result = await accountClient.RemoveLoginAsync(removeLogin);
                }
                
                if (result.Succeeded)
                {
                    AccountsItemControl.Items.Remove(account);
                }
                else
                {
                    ErrorDialog.ShowErrors(result.Errors);
                }
            }
        }
    }
}