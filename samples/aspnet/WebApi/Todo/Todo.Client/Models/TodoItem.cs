namespace Todo.Client.Models
{
    public class TodoItem
    {
        public int TodoItemId { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public int TodoListId { get; set; }
        public TodoList TodoList { get; set; }
    }
}
