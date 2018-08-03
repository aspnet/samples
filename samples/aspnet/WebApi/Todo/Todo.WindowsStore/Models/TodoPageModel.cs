using System.Collections.ObjectModel;
using Todo.Client.Models;

namespace Todo.WindowsStore.Models
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
    }
}
