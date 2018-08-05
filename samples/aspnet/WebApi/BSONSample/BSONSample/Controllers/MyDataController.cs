using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BSONSample.Models;

namespace BSONSample.Controllers
{
    public class MyDataController : ApiController
    {
        private MyDataContext db = new MyDataContext();

        // GET api/MyData
        public IQueryable<MyData> GetMyDatas()
        {
            return db.MyDatas;
        }

        // GET api/MyData/5
        [ResponseType(typeof(MyData))]
        public IHttpActionResult GetMyData(int id)
        {
            MyData mydata = db.MyDatas.Find(id);
            if (mydata == null)
            {
                return NotFound();
            }

            return Ok(mydata);
        }

        // PUT api/MyData/5
        public IHttpActionResult PutMyData(int id, MyData mydata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != mydata.Id)
            {
                return BadRequest();
            }

            db.Entry(mydata).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MyDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/MyData
        [ResponseType(typeof(MyData))]
        public IHttpActionResult PostMyData(MyData mydata)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.MyDatas.Add(mydata);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = mydata.Id }, mydata);
        }

        // DELETE api/MyData/5
        [ResponseType(typeof(MyData))]
        public IHttpActionResult DeleteMyData(int id)
        {
            MyData mydata = db.MyDatas.Find(id);
            if (mydata == null)
            {
                return NotFound();
            }

            db.MyDatas.Remove(mydata);
            db.SaveChanges();

            return Ok(mydata);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MyDataExists(int id)
        {
            return db.MyDatas.Count(e => e.Id == id) > 0;
        }
    }
}