using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using ODataActionsSample.Models;

namespace ODataActionsSample.Controllers
{
    public class MoviesController : EntitySetController<Movie, int>
    {
        private MoviesContext db = new MoviesContext();

        private bool TryCheckoutMovie(Movie movie)
        {
            if (movie.IsCheckedOut)
            {
                return false;
            }
            else
            {
                // To check out a movie, set the due date.
                movie.DueDate = DateTime.Now.AddDays(7);
                return true;
            }
        }

        // 
        public override IQueryable<Movie> Get()
        {
            return db.Movies;
        }

        protected override Movie GetEntityByKey(int key)
        {
            return db.Movies.Find(key);
        }

        #region Action methods

        [HttpPost]
        public Movie CheckOut([FromODataUri] int key)
        {
            var movie = GetEntityByKey(key);
            if (movie == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (!TryCheckoutMovie(movie))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return movie;
        }

        [HttpPost]
        public Movie Return([FromODataUri] int key)
        {
            var movie = GetEntityByKey(key);
            if (movie == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            movie.DueDate = null;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return movie;
        }

        [HttpPost]
        public Movie SetDueDate([FromODataUri] int key, ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var movie = GetEntityByKey(key);
            if (movie == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            movie.DueDate = (DateTime)parameters["DueDate"];
            // In a real app you would validate this date (not in the past, etc).
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return movie;
        }

        // Check out a list of movies.
        [HttpPost]
        public ICollection<Movie> CheckOutMany(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Client passes a list of movie IDs to check out.
            var movieIDs = new HashSet<int>(parameters["MovieIDs"] as IEnumerable<int>);

            // Try to check out each movie in the list.
            var results = new List<Movie>();
            foreach (Movie movie in db.Movies.Where(m => movieIDs.Contains(m.ID)))
            {
                if (TryCheckoutMovie(movie))
                {
                    results.Add(movie);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Return a list of the movies that were checked out.
            return results;
        }

        [HttpPost]
        // This action accepts $filter queries. For example:
        //     ~/odata/Movies/CheckOut?$filter=Year eq 2005
        public ICollection<Movie> CheckOut(ODataQueryOptions opts)
        {
            // Validate the query options.
            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.Filter
            };
            opts.Validate(settings);

            // Use the query options to get a filtered list of movies.
            var movies = opts.ApplyTo(db.Movies) as IQueryable<Movie>;

            // Try to check out each movie in the list.
            var results = new List<Movie>();
            foreach (Movie movie in movies)
            {
                if (TryCheckoutMovie(movie))
                {
                    results.Add(movie);
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            // Return a list of the movies that were checked out.
            return results;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
