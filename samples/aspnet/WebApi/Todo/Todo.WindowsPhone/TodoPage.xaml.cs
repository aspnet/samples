using Account.Client;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Todo.Client;
using Todo.Client.Models;
using Todo.WindowsPhone.Models;
using WebApi.Client;

namespace Todo.WindowsPhone
{
    public partial class TodoPage : PhoneApplicationPage
    {
        ApplicationBarIconButton deleteButton;
        string todoTitle;

        public TodoPage()
        {
            InitializeComponent();
            this.TodoPageModel = new TodoPageModel();
            this.DataContext = this.TodoPageModel;
            this.deleteButton = new ApplicationBarIconButton()
            {
                Text = "Delete",
                IconUri = new Uri("/Assets/appbar.delete.png", UriKind.Relative),
                IsEnabled = false
            };
            this.deleteButton.Click += DeleteTodoItemButton_Click;
            ApplicationBar.Buttons.Add(deleteButton);
        }

        public TodoPageModel TodoPageModel { get; private set; }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            TodoPageModel.TodoLists.Clear();
            HttpResult<UserInfo> userInfoResult;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                userInfoResult = await accountClient.GetUserInfoAsync();
            }

            if (userInfoResult.Succeeded)
            {
                UserInfo userInfo = userInfoResult.Content;
                TodoPageModel.Username = String.Format("{0}", userInfo.UserName);

                HttpResult<TodoList[]> todoListResult;
                using (TodoClient todoClient = ClientFactory.CreateTodoClient())
                {
                    todoListResult = await todoClient.GetTodoListsAsync();
                }

                if (todoListResult.Succeeded)
                {
                    if (todoListResult.Content.Length == 0)
                    {
                        this.NavigationService.Navigate(AddTodoListPage.GetNavigationUri());
                    }
                    else
                    {
                        foreach (TodoList todoList in todoListResult.Content)
                        {
                            TodoPageModel.TodoLists.Add(new TodoListModel(todoList));
                        }
                    }
                }
                else
                {
                    ErrorDialog.ShowErrors(todoListResult.Errors);
                }
            }
            else
            {
                ErrorDialog.ShowErrors(userInfoResult.Errors);
            }
        }

        void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        private void AddTodoListMenuItem_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/AddTodoListPage.xaml", UriKind.Relative));
        }

        private async void DeleteTodoListMenuItem_Click(object sender, EventArgs e)
        {
            TodoListModel todoList = GetSelectedTodoList();

            HttpResult result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.DeleteTodoListAsync(todoList.TodoListId);
            }

            if (result.Succeeded)
            {
                this.TodoPageModel.TodoLists.Remove(todoList);
            }
            else
            {
                ErrorDialog.ShowErrors(result.Errors);
            }
        }

        private void EditTodoListMenuItem_Click(object sender, EventArgs e)
        {
            TodoListModel todoList = GetSelectedTodoList();
            if (todoList != null)
            {
                this.NavigationService.Navigate(EditTodoListPage.GetNavigationUri(todoList));
            }
        }

        private void AddTodoItemButton_Click(object sender, EventArgs e)
        {
            TodoListModel todoList = GetSelectedTodoList();
            this.NavigationService.Navigate(AddTodoItemPage.GetNavigationUri(todoList.TodoListId));
        }

        private async void DeleteTodoItemButton_Click(object sender, EventArgs e)
        {
            TodoItemModel todo = TodoPageModel.SelectedTodoItem;

            HttpResult result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.DeleteTodoAsync(todo.TodoItemId);
            }

            if (result.Succeeded)
            {
                TodoPageModel.SelectedTodoItem = null;
                GetSelectedTodoList().Todos.Remove(todo);
            }
            else
            {
                ErrorDialog.ShowErrors(result.Errors);
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            TodoPageModel.SelectedTodoItem = GetSelectedTodoItem(sender);
            deleteButton.IsEnabled = true;
            todoTitle = textBox.Text;
        } 

        private async void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            deleteButton.IsEnabled = false; 
            TextBox textBox = (TextBox)sender;
            TodoItemModel todo = GetSelectedTodoItem(sender);

            if (todoTitle != textBox.Text)
            {
                TodoItem todoItem = new TodoItem() { TodoItemId = todo.TodoItemId, TodoListId = todo.TodoListId, Title = textBox.Text };
                HttpResult result;
                using (TodoClient todoClient = ClientFactory.CreateTodoClient())
                {
                    result = await todoClient.UpdateTodoAsync(todoItem);
                }

                if (!result.Succeeded)
                {
                    ErrorDialog.ShowErrors(result.Errors);
                }
            }
        }

        private async void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            TodoItemModel todo = GetSelectedTodoItem(sender);
            TodoItem todoItem = new TodoItem() { TodoItemId = todo.TodoItemId, TodoListId = todo.TodoListId, IsDone = checkBox.IsChecked.Value };
            HttpResult result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.UpdateTodoAsync(todoItem);
            }

            if (!result.Succeeded)
            {
                ErrorDialog.ShowErrors(result.Errors);
            }
        }

        TodoListModel GetSelectedTodoList()
        {
            return (TodoListModel)TodoListsPivot.SelectedItem;
        }

        TodoItemModel GetSelectedTodoItem(object sender)
        {
            FrameworkElement element = (FrameworkElement)sender;
            return (TodoItemModel)element.DataContext;
        }
    }
}