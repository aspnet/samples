using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http;
using Todo.Client;
using Todo.Client.Models;
using Todo.Web;
using WebApi.Client;

namespace Todo.Tests
{
    [TestClass]
    public class TodoClientTest
    {
        TodoClient client;
        string username = "danroth27";
        string invalidUsername = "danroth28";
        int invalidTodoListId = 10;
        int invalidTodoItemId = 10;
        
        [TestInitialize]
        public void Init()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MessageHandlers.Add(new RequestContextHandler()
            {
                Principal = new GenericPrincipal(new GenericIdentity(username), null),
                OwinContext = new OwinContext()
            });
            WebApiConfig.Register(config);
            config.DependencyResolver = MockTodoRepository.CreateDependencyResolver();

            HttpServer server = new HttpServer(config);
            HttpClient httpClient = new HttpClient(server) { BaseAddress = new Uri("http://localhost/") };
            client = new TodoClient(httpClient);
        }

        [TestMethod]
        public async Task GetTodoLists()
        {
            HttpResult<TodoList[]> result = await client.GetTodoListsAsync();

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
            Assert.IsTrue(result.Content.Length > 0, "No todo lists received");
            Assert.IsTrue(result.Content[0].Todos.Count > 0, "No todo items received");
        }

        [TestMethod]
        public async Task GetTodoList()
        {
            int todoListId = 1;

            HttpResult<TodoList> result = await client.GetTodoListAsync(todoListId);

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
            Assert.AreEqual(result.Content.TodoListId, todoListId, "Received wrong todo list");
        }

        [TestMethod]
        public async Task AddTodoList()
        {
            TodoList todoList = new TodoList() { TodoListId = 2, Title="Nice to have" };

            HttpResult<TodoList> result = await client.AddTodoListAsync(todoList);

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
            Assert.AreEqual(todoList.TodoListId, result.Content.TodoListId, "New TodoList ID does not match.");
            Assert.AreEqual(todoList.Title, result.Content.Title, "New TodoList title does not match");
            Assert.AreEqual(result.Content.UserId, username, "New TodoList user ID does not match");
        }

        [TestMethod]
        public async Task AddTodoListWithDifferentUserId()
        {
            TodoList todoList = new TodoList() { TodoListId = 2, Title = "Nice to have", UserId = invalidUsername };

            HttpResult<TodoList> result = await client.AddTodoListAsync(todoList);

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
            Assert.AreEqual(result.Content.UserId, username);
        }

        [TestMethod]
        public async Task UpdateTodoList()
        {
            string newTitle = "Nice to have";
            TodoList update = new TodoList() { TodoListId = 1, Title = newTitle, UserId = username };

            HttpResult<TodoList> result = await client.UpdateTodoListAsync(update, returnContent: true);

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
            Assert.AreEqual(result.Content.Title, newTitle);
        }

        [TestMethod]
        public async Task DeleteTodoList()
        {
            HttpResult result = await client.DeleteTodoListAsync(1);

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
        }

        [TestMethod]
        public async Task DeleteTodoListNotFound()
        {
            HttpResult result = await client.DeleteTodoListAsync(invalidTodoListId);

            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NotFound, "Expected status code Not Found");
        }

        [TestMethod]
        public async Task AddTodo()
        {
            TodoItem todo = new TodoItem() { TodoItemId = 2, Title = "Eat", IsDone = false, TodoListId = 1 };

            HttpResult<TodoItem> result = await client.AddTodoItemAsync(todo);

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
            Assert.AreEqual(result.Content.TodoItemId, todo.TodoItemId);
            Assert.AreEqual(result.Content.Title, todo.Title);
            Assert.AreEqual(result.Content.IsDone, todo.IsDone);
            Assert.AreEqual(result.Content.TodoListId, todo.TodoListId);
        }

        [TestMethod]
        public async Task UpdateTodo()
        {
            string newTitle = "Eat";
            TodoItem update = new TodoItem() { TodoItemId = 1, Title = newTitle };

            HttpResult<TodoItem> result = await client.UpdateTodoAsync(update, returnContent: true);

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
            Assert.AreEqual(result.Content.Title, newTitle);
        }

        [TestMethod]
        public async Task DeleteTodo()
        {
            HttpResult result = await client.DeleteTodoAsync(1);

            Assert.IsNotNull(result, "Result is null");
            Assert.IsTrue(result.Succeeded, GetRequestFailedMessage(result));
        }

        [TestMethod]
        public async Task DeleteTodoNotFound()
        {
            HttpResult result = await client.DeleteTodoAsync(invalidTodoItemId);

            Assert.IsNotNull(result, "Result is null");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NotFound, "Expected status code Not Found");
        }

        string GetRequestFailedMessage(HttpResult result)
        {
            return result.StatusCode != default(HttpStatusCode) ? String.Format("Request failed: {0}", result.StatusCode) : "Request failed.";
        }
    }
}
