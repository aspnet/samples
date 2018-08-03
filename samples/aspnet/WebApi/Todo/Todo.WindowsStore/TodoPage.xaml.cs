using Account.Client;
using System;
using System.Threading.Tasks;
using Todo.Client;
using Todo.Client.Models;
using Todo.WindowsStore.Common;
using Todo.WindowsStore.Models;
using WebApi.Client;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Todo.WindowsStore
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class TodoPage : Page
    {
        NavigationHelper navigationHelper;
        ObservableDictionary defaultViewModel = new ObservableDictionary();
        AccountSettings accountSettings;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public TodoPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.TodoPageModel = new TodoPageModel();
            this.DataContext = this.TodoPageModel;
            this.accountSettings = new AccountSettings();
            SettingsPane.GetForCurrentView().CommandsRequested += accountSettings.CommandsRequested;
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += accountSettings.AccountCommandsRequested;
        }

        public TodoPageModel TodoPageModel { get; private set; }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            TodoPageModel.TodoLists.Clear();
            HttpResult<UserInfo> result;
            using (AccountClient accountClient = ClientFactory.CreateAccountClient())
            {
                result = await accountClient.GetUserInfoAsync();
            }

            if (result.Succeeded)
            {
                UserInfo userInfo = result.Content;
                this.pageTitle.Text = String.Format("{0}'s Todos", userInfo.UserName);

                await GetTodoListsAsync();
            }
            else
            {
                await ErrorDialog.ShowErrorsAsync(result.Errors);
            }
        }

        async Task GetTodoListsAsync()
        {
            TodoPageModel.TodoLists.Clear();
            HttpResult<TodoList[]> todoListResult;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                todoListResult = await todoClient.GetTodoListsAsync();
            }

            if (todoListResult.Succeeded)
            {
                foreach (TodoList todoList in todoListResult.Content)
                {
                    TodoPageModel.TodoLists.Add(new TodoListModel(todoList));
                }
            }
            else
            {
                await ErrorDialog.ShowErrorsAsync(todoListResult.Errors);
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            if (accountSettings != null)
            {
                SettingsPane.GetForCurrentView().CommandsRequested -= accountSettings.CommandsRequested;
                AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested -= accountSettings.AccountCommandsRequested;
            }
        }

        private async void RefreshAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            await GetTodoListsAsync();
        }

        private async void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            HttpResult<TodoList> result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.AddTodoListAsync(new TodoList() { Title = "New Todo List" });
            }

            if (result.Succeeded)
            {
                TodoPageModel.TodoLists.Add(new TodoListModel(result.Content));
            }
            else
            {
                await ErrorDialog.ShowErrorsAsync(result.Errors);
            }
        }

        private async void DeleteAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            TodoListModel todoList = GetSelectedTodoList();

            HttpResult result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.DeleteTodoListAsync(todoList.TodoListId);
            }

            if (result.Succeeded)
            {
                TodoPageModel.TodoLists.Remove(todoList);
            }
            else
            {
                await ErrorDialog.ShowErrorsAsync(result.Errors);
            }
        }

        T GetDataContext<T>(object sender)
        {
            FrameworkElement element = (FrameworkElement)sender;
            return (T)element.DataContext;
        }

        private void TodoListGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isEnabled = e.AddedItems.Count > 0;
            DeleteAppBarButton.IsEnabled = isEnabled;
        }

        private void TodoListTitleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TodoListModel todoList = GetDataContext<TodoListModel>(sender);
            if (!String.IsNullOrWhiteSpace(todoList.Title))
            {
                todoList.OriginalTitle = todoList.Title;
            }
        }

        private async void TodoListTitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox todoListTitleTextBox = (TextBox)sender;
            TodoListModel todoList = GetDataContext<TodoListModel>(sender);

            string newTitle = todoListTitleTextBox.Text;
            if (newTitle != todoList.OriginalTitle)
            {
                HttpResult result;
                using (TodoClient todoClient = ClientFactory.CreateTodoClient())
                {
                    if (String.IsNullOrWhiteSpace(newTitle))
                    {
                        result = await todoClient.DeleteTodoListAsync(todoList.TodoListId);

                        if (result.Succeeded)
                        {
                            TodoPageModel.TodoLists.Remove(todoList);
                        }
                    }
                    else
                    {
                        TodoList update = new TodoList() { TodoListId = todoList.TodoListId, Title = newTitle };
                        result = await todoClient.UpdateTodoListAsync(update);

                        if (!result.Succeeded)
                        {
                            todoList.Title = todoList.OriginalTitle;
                        }
                    }
                }

                if (!result.Succeeded)
                {
                    await ErrorDialog.ShowErrorsAsync(result.Errors);
                }
            }
        }

        private void TodoTitleTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TodoItemModel todo = GetDataContext<TodoItemModel>(sender);
            todo.OriginalTitle = todo.Title;
        }

        private async void TodoTitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox todoTitleTextBox = (TextBox)sender;
            TodoItemModel todo = GetDataContext<TodoItemModel>(sender);
            string newTitle = todoTitleTextBox.Text;

            if (newTitle != todo.OriginalTitle)
            {
                HttpResult result;
                using (TodoClient todoClient = ClientFactory.CreateTodoClient())
                {
                    if (String.IsNullOrWhiteSpace(newTitle))
                    {
                        result = await todoClient.DeleteTodoAsync(todo.TodoItemId);

                        if (result.Succeeded)
                        {
                            todo.TodoList.Todos.Remove(todo);
                        }
                    }
                    else
                    {
                        TodoItem update = new TodoItem() { TodoItemId = todo.TodoItemId, TodoListId = todo.TodoListId, Title = newTitle };
                        result = await todoClient.UpdateTodoAsync(update);

                        if (!result.Succeeded)
                        {
                            todo.Title = todo.OriginalTitle;
                        }
                    }

                }

                if (!result.Succeeded)
                {
                    await ErrorDialog.ShowErrorsAsync(result.Errors);
                }
            }
        }

        private void NewTodoTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox newTodoTextBox = (TextBox)sender;
            newTodoTextBox.Text = String.Empty;
        }

        private async void NewTodoTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox newTodoTextBox = (TextBox)sender;
            TodoListModel todoList = GetDataContext<TodoListModel>(sender);
            if (!String.IsNullOrWhiteSpace(newTodoTextBox.Text))
            {
                HttpResult<TodoItem> result;
                using (TodoClient todoClient = ClientFactory.CreateTodoClient())
                {
                    result = await todoClient.AddTodoItemAsync(new TodoItem() { TodoListId = todoList.TodoListId, Title = newTodoTextBox.Text });
                }

                if (result.Succeeded)
                {
                    todoList.Todos.Add(new TodoItemModel(result.Content) { TodoList = todoList });
                }
                else
                {
                    await ErrorDialog.ShowErrorsAsync(result.Errors);
                }
            }
            newTodoTextBox.Text = "Add New Todo";
        }

        private async void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            TodoItemModel todo = GetDataContext<TodoItemModel>(sender);
            TodoItem todoItem = new TodoItem() { TodoItemId = todo.TodoItemId, TodoListId = todo.TodoListId, IsDone = checkBox.IsChecked.Value };
            HttpResult result;
            using (TodoClient todoClient = ClientFactory.CreateTodoClient())
            {
                result = await todoClient.UpdateTodoAsync(todoItem);
            }

            if (!result.Succeeded)
            {
                await ErrorDialog.ShowErrorsAsync(result.Errors);
            }
        }

        TodoListModel GetSelectedTodoList()
        {
            return (TodoListModel)TodoListGridView.SelectedItem;
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
