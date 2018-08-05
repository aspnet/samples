using Account.Client;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class LoginPage : PhoneApplicationPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            ExternalLoginsItemsControl.Items.Clear();

            HttpResult<ExternalLogin[]> result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.GetExternalLoginsAsync();
            }
            
            if (result.Succeeded)
            {
                ExternalLogin[] externalLogins = result.Content;
                ExternalLoginsPivotItem.Visibility = externalLogins.Length > 0 ? Visibility.Visible : Visibility.Collapsed;
                foreach (ExternalLogin externalLogin in externalLogins)
                {
                    string provider = externalLogin.Name;
                    Account account = new Account()
                    {
                        Provider = provider,
                        ProviderUri = externalLogin.Url,
                        Icon = Account.GetAccountIcon(provider)
                    };
                    ExternalLoginsItemsControl.Items.Add(account);
                }
            }
            else
            {
                ExternalLoginsPivotItem.Visibility = Visibility.Collapsed;
                ErrorDialog.ShowErrors(result.Errors);
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ClearLoginErrors();

            HttpResult<AccessTokenResponse> result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.LoginAsync(UsernameTextBox.Text, UserPasswordBox.Password);
            }
            
            if (result.Succeeded)
            {
                string accessToken = result.Content.AccessToken;
                bool completed = AccessTokenProvider.AccessTokenSource.TrySetResult(accessToken);
                this.NavigationService.Navigate(new Uri("/TodoPage.xaml", UriKind.Relative));
            }
            else
            {
                DisplayLoginErrors(result.Errors);
            }
        }

        void DisplayLoginErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                LoginErrorList.Items.Add(error);
            }
        }

        void ClearLoginErrors()
        {
            LoginErrorList.Items.Clear();
        }

        private async void ExternalLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Account account = (Account)((FrameworkElement)sender).DataContext;
            ExternalLoginResult loginExternalResult = await ExternalLoginManager.GetExternalAccessTokenAsync(account.ProviderUri);

            if (loginExternalResult.AccessToken != null)
            {
                AppSettings settings = new AppSettings();
                settings.AccessToken = loginExternalResult.AccessToken;

                HttpResult<UserInfo> result;
                using (AccountClient accountClient = ClientFactory.CreateAccountClient())
                {
                    result = await accountClient.GetUserInfoAsync();
                }

                if (result.Succeeded)
                {
                    UserInfo userInfo = result.Content;
                    if (userInfo.HasRegistered)
                    {
                        bool completed = AccessTokenProvider.AccessTokenSource.TrySetResult(loginExternalResult.AccessToken);
                        this.NavigationService.Navigate(new Uri("/TodoPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        this.NavigationService.Navigate(RegisterExternalPage.GetNavigationUri(userInfo.LoginProvider, userInfo.UserName, account.ProviderUri));
                    }
                }
                else
                {
                    ErrorDialog.ShowErrors("Failed to connect to external account.", result.Errors);
                }

            }
            else
            {
                ErrorDialog.ShowError("Failed to connect to external account.");
            }
        }

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            ClearSignUpErrors();

            // TODO: Add input validation
            RegisterUser registerUser = new RegisterUser()
            {
                UserName = this.NewUsernameTextBox.Text,
                Password = this.NewPasswordBox.Password,
                ConfirmPassword = this.ConfirmPasswordBox.Password
            };

            HttpResult registerResult;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                registerResult = await accountClient.RegisterAsync(registerUser);
                if (registerResult.Succeeded)
                {
                    HttpResult<AccessTokenResponse> loginResult = await accountClient.LoginAsync(registerUser.UserName, registerUser.Password);
                    if (loginResult.Succeeded)
                    {
                        string accessToken = loginResult.Content.AccessToken;
                        bool completed = AccessTokenProvider.AccessTokenSource.TrySetResult(accessToken);
                        this.NavigationService.Navigate(new Uri("/TodoPage.xaml", UriKind.Relative));
                    }
                    else
                    {
                        DisplaySignUpErrors(loginResult.Errors);
                    }
                }
                else
                {
                    DisplaySignUpErrors(registerResult.Errors);
                }
            }
        }

        void DisplaySignUpErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                SignUpErrorList.Items.Add(error);
            }
        }

        void ClearSignUpErrors()
        {
            SignUpErrorList.Items.Clear();
        }
    }
}