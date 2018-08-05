using Autofac;
using Autofac.Integration.WebApi;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dependencies;
using Todo.Web.Controllers;
using Todo.Web.Models;

namespace Todo.Tests
{
    public static class MockTodoRepository
    {
        public static IDependencyResolver CreateDependencyResolver()
        {
            TodoList todoList = new TodoList() { Title = "Important", TodoListId = 1, UserId = "danroth27" }; 
            var todoLists = new List<TodoList>() { todoList }.AsQueryable();
            todoList.Todos = new List<TodoItem>()
            {
                new TodoItem() { Title = "Sleep", IsDone = false, TodoItemId = 1, TodoListId = 1, TodoList = todoList }
            };

            var mockContext = new Mock<TodosDbContext>();
            var mockTodoListSet = GetMockDbSet(todoLists);
            var mockTodoItemSet = GetMockDbSet(todoList.Todos.AsQueryable());
            mockContext.Setup(m => m.TodoLists).Returns(mockTodoListSet.Object);
            mockContext.Setup(m => m.TodoItems).Returns(mockTodoItemSet.Object);

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetAssembly(typeof(TodosController)));
            builder.Register(c => mockContext.Object);
            IContainer container = builder.Build();

            return new AutofacWebApiDependencyResolver(container);
        }

        static Mock<MockableDbSetWithIQueryable<T>> GetMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockDbSet = new Mock<MockableDbSetWithIQueryable<T>>();
            mockDbSet.Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mockDbSet;
        }
    }

    
}
