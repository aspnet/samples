using Account.Client;
using System.Collections.Generic;
using WebApi.Client;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Todo.WindowsStore
{
    public sealed partial class ChangePasswordFlyout : SettingsFlyout
    {
        string username;

        public ChangePasswordFlyout(string username)
        {
            this.InitializeComponent();
            this.username = username;
        }

        private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ResetDisplay();
            ChangePassword changePassword = new ChangePassword()
            {
                OldPassword = OldPasswordBox.Password,
                NewPassword = NewPasswordBox.Password,
                ConfirmPassword = ConfirmPasswordBox.Password
            };

            HttpResult result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.ChangePasswordAsync(changePassword);
            }

            if (result.Succeeded)
            {
                AppSettings settings = new AppSettings();
                settings.ChangePasswordCredential(username, changePassword.NewPassword);
                DisplaySuccess(); 
            }
            else
            {
                DisplayErrors(result.Errors);
            }
            ClearPasswords();
        }

        private void ResetDisplay()
        {
            SuccessTextBlock.Visibility = Visibility.Collapsed;
            ErrorList.Items.Clear();
        }

        private void ClearPasswords()
        {
            OldPasswordBox.Password = "";
            NewPasswordBox.Password = "";
            ConfirmPasswordBox.Password = "";
        }

        private void DisplaySuccess()
        {
            SuccessTextBlock.Visibility = Visibility.Visible;
        }

        private void DisplayErrors(IEnumerable<string> errors)
        {
            foreach (string error in errors)
            {
                ErrorList.Items.Add(error);
            }
        }
    }
}
