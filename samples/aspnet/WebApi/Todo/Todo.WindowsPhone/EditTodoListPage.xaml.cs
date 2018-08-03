using Microsoft.Phone.Controls;
using System;
using System.Net.Http.Formatting;
using System.Windows.Navigation;
using Todo.Client;
using Todo.Client.Models;
using Todo.WindowsPhone.Models;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class EditTodoListPage : PhoneApplicationPage
    {
        public const string TodoListId = "id";
        public const string TodoListTitle = "title";

        string todoListTitle;
        int todoListId;

        public EditTodoListPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.todoListTitle = this.NavigationContext.QueryString[TodoListTitle];
            this.TitleTextBox.Text = this.todoListTitle;
            this.todoListId = int.Parse(this.NavigationContext.QueryString[TodoListId]);
        }

        private async void OkButton_Click(object sender, EventArgs e)
        {
            TodoList todoList = new TodoList() { TodoListId = todoListId, Title = TitleTextBox.Text };

            HttpResult result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.UpdateTodoListAsync(todoList);
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

        public static Uri GetNavigationUri(TodoListModel todoList)
        {
            HttpValueCollection httpValues = new HttpValueCollection();
            httpValues.Add(TodoListTitle, todoList.Title);
            httpValues.Add(TodoListId, todoList.TodoListId.ToString());
            return new Uri("/EditTodoListPage.xaml?" + httpValues.ToString(), UriKind.Relative);
        }
    }
}