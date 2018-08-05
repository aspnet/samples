using System.Collections.ObjectModel;

namespace Todo.Client.Models
{
    public class TodoList
    {
        public int TodoListId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public Collection<TodoItem> Todos { get; set; }
    }
}
