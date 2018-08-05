using Todo.Client.Models;

namespace Todo.WindowsPhone.Models
{
    public class TodoItemModel : BindableBase
    {
        int todoItemId;
        string title;
        bool isDone;
        int todoListId;

        public TodoItemModel(TodoItem todo)
        {
            todoItemId = todo.TodoItemId;
            title = todo.Title;
            isDone = todo.IsDone;
            todoListId = todo.TodoListId;
        }

        public int TodoItemId
        {
            get
            {
                return todoItemId;
            }
            set
            {
                SetProperty(ref todoItemId, value);
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

        public bool IsDone
        {
            get
            {
                return isDone;
            }
            set
            {
                SetProperty(ref isDone, value);
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

        public TodoItem ToTodoItem()
        {
            return new TodoItem()
            {
                TodoItemId = todoItemId,
                Title = title,
                IsDone = isDone,
                TodoListId = todoListId
            };
        }
    }
}
