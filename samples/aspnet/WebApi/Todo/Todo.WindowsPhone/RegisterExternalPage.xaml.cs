using Account.Client;
using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Windows;
using System.Windows.Navigation;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class RegisterExternalPage : PhoneApplicationPage
    {
        public const string Username = "username";
        public const string Provider = "provider";
        public const string ExternalLoginUri = "externalLoginUri";

        string username;
        string provider;
        string externalLoginUri;    

        public RegisterExternalPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.username = this.NavigationContext.QueryString[Username];
            this.provider = this.NavigationContext.QueryString[Provider];
            this.externalLoginUri = this.NavigationContext.QueryString[ExternalLoginUri];

            string text = this.SuccessfulExternalLoginTextBlock.Text;
            this.SuccessfulExternalLoginTextBlock.Text =
                text.Substring(0, text.LastIndexOf(' ') + 1) + provider + '.';
            this.UsernameTextBox.Text = username;
        }

        private async void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();

            RegisterExternalUser registerExternalUser = new RegisterExternalUser()
            {
                UserName = this.UsernameTextBox.Text,
            };

            HttpResult result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.RegisterExternalAsync(registerExternalUser);
            }

            if (result.Succeeded)
            {
                // Need to login again now that we are registered - should happen with the existing cookie
                ExternalLoginResult externalLoginResult = await ExternalLoginManager.GetExternalAccessTokenAsync(externalLoginUri);
                bool completed = AccessTokenProvider.AccessTokenSource.TrySetResult(externalLoginResult.AccessToken);
                this.NavigationService.Navigate(new Uri("/TodoPage.xaml", UriKind.Relative));
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
                ErrorList.Items.Add(error);
            }
        }

        void ClearErrors()
        {
            ErrorList.Items.Clear();
        }

        public static Uri GetNavigationUri(string provider, string username, string externalLoginUri)
        {
            HttpValueCollection httpValues = new HttpValueCollection();
            httpValues.Add(Provider, provider);
            httpValues.Add(Username, username);
            httpValues.Add(ExternalLoginUri, externalLoginUri);
            return new Uri("/RegisterExternalPage.xaml?" + httpValues.ToString(), UriKind.Relative);
        }
    }
}