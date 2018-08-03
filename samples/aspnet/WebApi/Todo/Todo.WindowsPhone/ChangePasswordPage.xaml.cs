using Account.Client;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Windows;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class ChangePasswordPage : PhoneApplicationPage
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private async void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ClearErrors();
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
                this.NavigationService.GoBack();
            }
            else
            {
                DisplayErrors(result.Errors);
            }
            ClearPasswords();
        }

        private void ClearPasswords()
        {
            OldPasswordBox.Password = "";
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

        private void ClearErrors()
        {
            ErrorList.Items.Clear();
        }
    }
}