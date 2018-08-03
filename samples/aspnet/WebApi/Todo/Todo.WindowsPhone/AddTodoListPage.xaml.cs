using Microsoft.Phone.Controls;
using System;
using Todo.Client;
using Todo.Client.Models;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class AddTodoListPage : PhoneApplicationPage
    {
        public AddTodoListPage()
        {
            InitializeComponent();
        }

        private async void OkButton_Click(object sender, EventArgs e)
        {
            TodoList todoList = new TodoList() { Title = TitleTextBox.Text, UserId = "unknown"};

            HttpResult<TodoList> result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.AddTodoListAsync(todoList);
            }

            if (result.Succeeded)
            {
                this.NavigationService.GoBack();
            }
            else
            {
                ErrorDialog.ShowErrors(result.Errors);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.NavigationService.GoBack();
        }

        public static Uri GetNavigationUri()
        {
            return new Uri("/AddTodoListPage.xaml", UriKind.Relative);
        }
    }
}