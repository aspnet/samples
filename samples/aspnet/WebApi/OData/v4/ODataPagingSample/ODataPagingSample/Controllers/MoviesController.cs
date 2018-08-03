using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataPagingSample.Models;

namespace ODataPagingSample.Controllers
{
    public class MoviesController : ODataController
    {
        private MoviesDb db = new MoviesDb();

        // GET: odata/Movies
        [EnableQuery(PageSize = 10)]
        public IQueryable<Movie> GetMovies()
        {
            return db.Movies;
        }

        // GET: odata/Movies(5)
        [EnableQuery]
        public SingleResult<Movie> GetMovie([FromODataUri] int key)
        {
            return SingleResult.Create(db.Movies.Where(movie => movie.ID == key));
        }

        // PUT: odata/Movies(5)
        public IHttpActionResult Put([FromODataUri] int key, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (key != movie.ID)
            {
                return BadRequest();
            }

            db.Entry(movie).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(movie);
        }

        // POST: odata/Movies
        public IHttpActionResult Post(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Movies.Add(movie);
            db.SaveChanges();

            return Created(movie);
        }

        // PATCH: odata/Movies(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Movie> patch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Movie movie = db.Movies.Find(key);
            if (movie == null)
            {
                return NotFound();
            }

            patch.Patch(movie);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(movie);
        }

        // DELETE: odata/Movies(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Movie movie = db.Movies.Find(key);
            if (movie == null)
            {
                return NotFound();
            }

            db.Movies.Remove(movie);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MovieExists(int key)
        {
            return db.Movies.Count(e => e.ID == key) > 0;
        }
    }
}
