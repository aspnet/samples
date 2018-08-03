using System.Collections.ObjectModel;
using System.Linq;
using Todo.Client.Models;

namespace Todo.WindowsStore.Models
{
    public class TodoListModel : BindableBase
    {
        int todoListId;
        string userId;
        string title;
        ObservableCollection<TodoItemModel> todos;

        public TodoListModel()
        {
            this.todos = new ObservableCollection<TodoItemModel>();
        }
        public TodoListModel(TodoList todoList) : this()
        {
            todoListId = todoList.TodoListId;
            userId = todoList.UserId;
            title = todoList.Title;
            if (todoList.Todos != null)
            {
                todos = new ObservableCollection<TodoItemModel>(todoList.Todos.Select(todo => new TodoItemModel(todo) { TodoList = this }));
            }
        }

        public int TodoListId
        {
            get
            {
                return todoListId;
            }
            set
            {
                SetProperty(ref todoListId, value);
            }
        }
        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                SetProperty(ref userId, value);
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                SetProperty(ref title, value);
            }
        }

        public ObservableCollection<TodoItemModel> Todos
        {
            get
            {
                return todos;
            }
        }

        public TodoList ToTodoList()
        {
            return new TodoList()
            {
                TodoListId = todoListId,
                Title = title,
                UserId = userId,
                Todos = new Collection<TodoItem>(Todos.Select(todo => todo.ToTodoItem()).ToList())
            };
        }

        public string OriginalTitle { get; set; }
    }
}
