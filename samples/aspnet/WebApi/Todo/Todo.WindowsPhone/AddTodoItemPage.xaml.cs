using Microsoft.Phone.Controls;
using System;
using System.Net.Http.Formatting;
using System.Windows.Navigation;
using Todo.Client;
using Todo.Client.Models;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class AddTodoItemPage : PhoneApplicationPage
    {
        public const string TodoListId = "todoListId";

        int todoListId;

        public AddTodoItemPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.todoListId = int.Parse(this.NavigationContext.QueryString[TodoListId]);
        }

        private async void OkButton_Click(object sender, EventArgs e)
        {
            TodoItem todoItem = new TodoItem()
            {
                Title = TitleTextBox.Text,
                TodoListId = todoListId
            };

            HttpResult<TodoItem> result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.AddTodoItemAsync(todoItem);
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

        public static Uri GetNavigationUri(int todoListId)
        {
            HttpValueCollection values = new HttpValueCollection();
            values.Add(TodoListId, todoListId.ToString());
            return new Uri("/AddTodoItemPage.xaml?" + values.ToString(), UriKind.Relative);
        }
    }
}