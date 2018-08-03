using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using ODataActionsSample.Models;

namespace ODataActionsSample.Controllers
{
    // Controller for handling non-bindable actions.
    public class NonBindableActionsController : ODataController
    {
        MoviesContext db = new MoviesContext();

        [HttpPost]
        public Movie CreateMovie(ODataActionParameters parameters)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            string title = parameters["Title"] as string;

            Movie movie = new Movie()
            {
                Title = title
            };

            try
            {
                db.Movies.Add(movie);
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            return movie;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}