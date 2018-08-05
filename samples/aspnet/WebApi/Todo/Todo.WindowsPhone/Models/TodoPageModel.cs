using System.Collections.ObjectModel;
using Todo.Client.Models;

namespace Todo.WindowsPhone.Models
{
    public class TodoPageModel : BindableBase
    {
        string username;

        public TodoPageModel()
        {
            this.TodoLists = new ObservableCollection<TodoListModel>();
        }

        public string Username
        {
            get
            {
                return username;
            }
            set 
            { 
                SetProperty(ref username, value); 
            }
        }

        public ObservableCollection<TodoListModel> TodoLists { get; private set; }
        public TodoItemModel SelectedTodoItem { get; set; }
    }
}
