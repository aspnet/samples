using Account.Client;
using System.Collections.Generic;
using WebApi.Client;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Todo.WindowsStore
{
    public sealed partial class SetPasswordFlyout : SettingsFlyout
    {
        string username;

        public SetPasswordFlyout(string username)
        {
            this.InitializeComponent();
            this.username = username;
        }

        private async void SetPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ResetDisplay();
            SetPassword setPassword = new SetPassword()
            {
                NewPassword = NewPasswordBox.Password,
                ConfirmPassword = ConfirmPasswordBox.Password
            };

            HttpResult result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.SetPasswordAsync(setPassword);
            }

            if (result.Succeeded)
            {
                AppSettings settings = new AppSettings();
                settings.SavePasswordCredential(this.username, setPassword.NewPassword);
                AccountsSettingsPane.Show();
            }
            else
            {
                DisplayErrors(result.Errors);
            }
            ClearPasswords();

        }

        private void ResetDisplay()
        {
            ErrorList.Items.Clear();
        }

        private void ClearPasswords()
        {
            NewPasswordBox.Password = "";
            ConfirmPasswordBox.Password = "";
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
