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
    public class TodoListsController : ODataController
    {
        private TodosDbContext db;

        public TodoListsController() : this(new TodosDbContext())
        {
        }

        public TodoListsController(TodosDbContext context)
        {
            this.db = context;
        }

        // GET odata/TodoLists
        [Queryable]
        public IQueryable<TodoList> GetTodoLists()
        {
            return db.TodoLists.Where(list => list.UserId == User.Identity.Name);
        }

        // GET odata/TodoLists(5)
        [Queryable]
        public SingleResult<TodoList> GetTodoList([FromODataUri] int key)
        {
            return SingleResult.Create(GetTodoListQuery(key));
        }

        // POST odata/TodoLists
        public IHttpActionResult Post(TodoList todoList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            todoList.UserId = User.Identity.Name;
            db.TodoLists.Add(todoList);
            db.SaveChanges();

            return Created(todoList);
        }

        // PUT odata/TodoLists(5)
        public IHttpActionResult Put([FromODataUri] int key, TodoList todoList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != todoList.TodoListId || todoList.UserId != User.Identity.Name)
            {
                return BadRequest();
            }

            if (!GetTodoListQuery(key).Any())
            {
                return NotFound();
            }

            db.Entry(todoList).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(todoList);
        }

        // PATCH odata/TodoLists(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<TodoList> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            dynamic patchedTodoList = patch;
            if (key != patchedTodoList.TodoListId)
            {
                return BadRequest();
            }

            TodoList todoList = FindTodoList(key);
            if (todoList == null)
            {
                return NotFound();
            }

            patchedTodoList.UserId = User.Identity.Name;
            patch.Patch(todoList);

            ModelStateDictionary modelState;
            if (!this.IsValid(todoList, "patch", out modelState))
            {
                return BadRequest(modelState);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(todoList);
        }

        // DELETE odata/TodoLists(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            TodoList todoList = FindTodoList(key);
            if (todoList == null)
            {
                return NotFound();
            }

            db.TodoLists.Remove(todoList);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET odata/TodoLists(5)/Todos
        [Queryable]
        public IHttpActionResult GetTodos([FromODataUri] int key)
        {
            TodoList todoList = db.TodoLists
                .Include(list => list.Todos)
                .Where(list => list.TodoListId == key && list.UserId == User.Identity.Name)
                .SingleOrDefault();	
            if (todoList == null)
            {
                return NotFound();
            }
            return Ok(todoList.Todos);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TodoListExists(int key)
        {
            return db.TodoLists.Count(e => e.TodoListId == key) > 0;
        }

        private IQueryable<TodoList> GetTodoListQuery(int key)
        {
            return db.TodoLists.Where(list => list.TodoListId == key && list.UserId == User.Identity.Name);
        }

        private TodoList FindTodoList(int key)
        {
            return GetTodoListQuery(key).SingleOrDefault();
        }
    }
}
