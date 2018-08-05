using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TouristAttractions.Models;

namespace TouristAttractions.Controllers
{
    public class AttractionsController : ApiController
    {
        private TourismContext db = new TourismContext();

        public TouristAttraction GetTouristAttraction(double longitude, double latitude)
        {
            var location = DbGeography.FromText(
                string.Format("POINT ({0} {1})", longitude, latitude));

            var query = from a in db.TouristAttractions
                        orderby a.Location.Distance(location)
                        select a;

            return query.FirstOrDefault();
        }


        // GET api/Attractions
        public IEnumerable<TouristAttraction> GetTouristAttractions()
        {
            return db.TouristAttractions.AsEnumerable();
        }

        // GET api/Attractions/5
        public TouristAttraction GetTouristAttraction(int id)
        {
            TouristAttraction touristattraction = db.TouristAttractions.Find(id);
            if (touristattraction == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return touristattraction;
        }

        // PUT api/Attractions/5
        public IHttpActionResult PutTouristAttraction(int id, TouristAttraction touristattraction)
        {
            if (ModelState.IsValid && id == touristattraction.TouristAttractionId)
            {
                db.Entry(touristattraction).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        // POST api/Attractions
        public IHttpActionResult PostTouristAttraction(TouristAttraction touristattraction)
        {
            if (ModelState.IsValid)
            {
                db.TouristAttractions.Add(touristattraction);
                db.SaveChanges();

                Uri location = new Uri(Url.Link("DefaultApi", new { id = touristattraction.TouristAttractionId }));
                return Created(location, touristattraction);
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE api/Attractions/5
        public IHttpActionResult DeleteTouristAttraction(int id)
        {
            TouristAttraction touristattraction = db.TouristAttractions.Find(id);
            if (touristattraction == null)
            {
                return NotFound();
            }

            db.TouristAttractions.Remove(touristattraction);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(touristattraction);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}