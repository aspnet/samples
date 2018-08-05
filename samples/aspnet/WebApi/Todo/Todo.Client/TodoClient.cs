using System;
using System.Net.Http;
using System.Threading.Tasks;
using Todo.Client.Models;
using WebApi.Client;

namespace Todo.Client
{
    public sealed class TodoClient : IDisposable
    {
        private const string Prefix = "odata/";
        private const string TodoListsUri = Prefix + "TodoLists";
        private const string TodosUri = Prefix + "Todos";

        private bool disposed;

        public TodoClient(HttpClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException("client");
            }
            HttpClient = client;
        }

        public HttpClient HttpClient { get; set; }

        public async Task<HttpResult<TodoList[]>> GetTodoListsAsync()
        {
            HttpResult<Wrapped<TodoList[]>> wrappedResult = await HttpClient.GetAsync<Wrapped<TodoList[]>>(TodoListsUri + "?$expand=Todos");
            HttpResult<TodoList[]> result = wrappedResult.Succeeded ? new HttpResult<TodoList[]>(wrappedResult.Content.Value) : HttpResult<TodoList[]>.Failure(wrappedResult.Errors);
            result.StatusCode = wrappedResult.StatusCode;
            return result;
        }

        public Task<HttpResult<TodoList>> GetTodoListAsync(int id)
        {
            ThrowIfDisposed();
            return HttpClient.GetAsync<TodoList>(TodoListsUri + "(" + id + ")");
        }

        public Task<HttpResult<TodoList>> AddTodoListAsync(TodoList todoList)
        {
            ThrowIfDisposed();
            todoList.UserId = "TBD"; // UserId is required, but is not known at this point
            return HttpClient.PostAsJsonAsync<TodoList, TodoList>(TodoListsUri, todoList);
        }

        public Task<HttpResult> DeleteTodoListAsync(int todoListId)
        {
            ThrowIfDisposed();
            return HttpClient.DeleteItemAsync(GetTodoListUri(todoListId));
        }

        public Task<HttpResult<TodoList>> UpdateTodoListAsync(TodoList todoList, bool returnContent = false)
        {
            ThrowIfDisposed();
            return HttpClient.PatchAsync(GetTodoListUri(todoList.TodoListId), todoList, returnContent);
        }

        public Task<HttpResult<TodoItem>> AddTodoItemAsync(TodoItem todo)
        {
            ThrowIfDisposed();
            return HttpClient.PostAsJsonAsync<TodoItem, TodoItem>(TodosUri, todo);
        }

        public Task<HttpResult> DeleteTodoAsync(int todoItemId)
        {
            ThrowIfDisposed();
            return HttpClient.DeleteItemAsync(GetTodoItemUri(todoItemId));
        }

        public Task<HttpResult<TodoItem>> UpdateTodoAsync(TodoItem todo, bool returnContent = false)
        {
            ThrowIfDisposed();
            return HttpClient.PatchAsync(GetTodoItemUri(todo.TodoItemId), todo, returnContent);
        }

        string GetTodoListUri(int todoListId)
        {
            return String.Format("{0}({1})", TodoListsUri, todoListId);
        }

        string GetTodoItemUri(int todoItemId)
        {
            return String.Format("{0}({1})", TodosUri, todoItemId);
        }

        public void Dispose()
        {
            if (!disposed)
            {
                HttpClient.Dispose();
                disposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }
    }
}
