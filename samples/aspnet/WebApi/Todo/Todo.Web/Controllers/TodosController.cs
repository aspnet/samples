using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using Todo.Web.Helpers;
using Todo.Web.Models;

namespace Todo.Web.Controllers
{
    [Authorize]
    public class TodosController : ODataController
    {
        TodosDbContext db;

        public TodosController() : this(new TodosDbContext())
        {
        }

        public TodosController(TodosDbContext context)
        {
            this.db = context;
        }

        // GET odata/Todos
        [Queryable]
        public IQueryable<TodoItem> GetTodos()
        {
            return db.TodoItems.Where(todo => todo.TodoList.UserId == User.Identity.Name);
        }

        // GET odata/Todos(5)
        [Queryable]
        public SingleResult<TodoItem> GetTodoItem([FromODataUri] int key)
        {
            return SingleResult.Create(GetTodoItemQuery(key));
        }

        // POST odata/Todos
        public IHttpActionResult Post(TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TodoList todoList = FindTodoList(todoItem.TodoListId);
            if (todoList == null)
            {
                return BadRequest("Todo list does not exist");
            }

            todoList.Todos.Add(todoItem);
            db.SaveChanges();

            return Created(todoItem);
        }

        // PUT odata/Todos(5)
        public IHttpActionResult Put([FromODataUri] int key, TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != todoItem.TodoItemId)
            {
                return BadRequest();
            }

            if (!GetTodoItemQuery(key).Any())
            {
                return NotFound();
            }

            db.Entry(todoItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(todoItem);
        }

        // PATCH odata/Todos(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<TodoItem> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dynamic patchedTodoItem = patch;
            if (key != patchedTodoItem.TodoItemId)
            {
                return BadRequest();
            }

            TodoItem todoItem = FindTodoItem(key);
            if (todoItem == null)
            {
                return NotFound();
            }

            patch.Patch(todoItem);

            ModelStateDictionary modelState;
            if (!this.IsValid(todoItem, "patch", out modelState))
            {
                return BadRequest(modelState);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(todoItem);
        }

        // DELETE odata/Todos(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            TodoItem todoItem = FindTodoItem(key);
            if (todoItem == null)
            {
                return NotFound();
            }

            db.TodoItems.Remove(todoItem);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/Todos(5)/TodoList
        [Queryable]
        public SingleResult<TodoList> GetTodoList([FromODataUri] int key)
        {
            return SingleResult.Create(db.TodoItems
                .Where(todo => todo.TodoItemId == key && todo.TodoList.UserId == User.Identity.Name)
                .Select(todo => todo.TodoList));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TodoItemExists(int key)
        {
            return db.TodoItems.Count(e => e.TodoItemId == key) > 0;
        }

        private IQueryable<TodoList> GetTodoListQuery(int key)
        {
            return db.TodoLists.Where(list => list.TodoListId == key && list.UserId == User.Identity.Name);
        }

        private TodoList FindTodoList(int key)
        {
            return GetTodoListQuery(key).SingleOrDefault();
        }

        private IQueryable<TodoItem> GetTodoItemQuery(int key)
        {
            return db.TodoItems.Where(todo => todo.TodoItemId == key && todo.TodoList.UserId == User.Identity.Name);
        }

        private TodoItem FindTodoItem(int key)
        {
            return GetTodoItemQuery(key).SingleOrDefault();
        }
    }
}
