using Microsoft.Phone.Controls;
using System;
using System.Net.Http.Formatting;
using System.Windows.Navigation;

namespace Todo.WindowsPhone
{
    public partial class AccountDetailsPage : PhoneApplicationPage
    {
        public static string AccountName = "accountName";
        public static string Username = "username";

        public AccountDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.AccountNameTextBlock.Text = this.NavigationContext.QueryString[AccountName];
            this.UsernameTextBlock.Text = this.NavigationContext.QueryString[Username];
        }

        public static Uri GetNavigationUri(Account account)
        {
            HttpValueCollection httpValues = new HttpValueCollection();
            httpValues.Add(AccountName, account.Provider);
            httpValues.Add(Username, account.ProviderKey);
            return new Uri("/AccountDetailsPage.xaml?" + httpValues.ToString(), UriKind.Relative);
        }

    }
}