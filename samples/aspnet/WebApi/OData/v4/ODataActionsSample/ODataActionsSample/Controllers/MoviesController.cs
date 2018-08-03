using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;
using ODataActionsSample.Models;

namespace ODataActionsSample.Controllers
{
    public class MoviesController : ODataController
    {
        private MoviesContext _db = new MoviesContext();

        public IHttpActionResult Get()
        {
            return Ok(_db.Movies);
        }

        [HttpPost]
        public IHttpActionResult CheckOut(int key)
        {
            var movie = _db.Movies.FirstOrDefault(m => m.ID == key);
            if (movie == null)
            {
                return BadRequest(ModelState);
            }

            if (!TryCheckoutMovie(movie))
            {
                return BadRequest("The movie is already checked out.");
            }

            return Ok(movie);
        }

        [HttpPost]
        public IHttpActionResult Return(int key)
        {
            var movie = _db.Movies.FirstOrDefault(m => m.ID == key);
            if (movie == null)
            {
                return BadRequest(ModelState);
            }

            movie.DueDate = null;

            return Ok(movie);
        }

        // Check out a list of movies.
        [HttpPost]
        public IHttpActionResult CheckOutMany(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Client passes a list of movie IDs to check out.
            var movieIDs = new HashSet<int>(parameters["MovieIDs"] as IEnumerable<int>);

            // Try to check out each movie in the list.
            var results = new List<Movie>();
            foreach (Movie movie in _db.Movies.Where(m => movieIDs.Contains(m.ID)))
            {
                if (TryCheckoutMovie(movie))
                {
                    results.Add(movie);
                }
            }

            // Return a list of the movies that were checked out.
            return Ok(results);
        }

        [HttpPost]
        [ODataRoute("CreateMovie")]
        public IHttpActionResult CreateMovie(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string title = parameters["Title"] as string;

            Movie movie = new Movie()
            {
                Title = title,
                ID = _db.Movies.Count + 1,
            };

            _db.Movies.Add(movie);

            return Created(movie);
        }

        protected Movie GetMovieByKey(int key)
        {
            return _db.Movies.FirstOrDefault(m => m.ID == key);
        }

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
    }
}
