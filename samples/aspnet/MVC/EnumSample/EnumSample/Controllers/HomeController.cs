using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EnumSample.Models;

namespace EnumSample.Controllers
{
    public class HomeController : Controller
    {
        private MyDataContext db = new MyDataContext();

        // GET: /Home/
        public ActionResult Index()
        {
            return View(db.MyDatas.ToList());
        }

        // GET: /Home/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyData mydata = db.MyDatas.Find(id);
            if (mydata == null)
            {
                return HttpNotFound();
            }
            return View(mydata);
        }

        // GET: /Home/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Home/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Enum1,Enum2,FlagsEnum")] MyData mydata)
        {
            if (ModelState.IsValid)
            {
                db.MyDatas.Add(mydata);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mydata);
        }

        // GET: /Home/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyData mydata = db.MyDatas.Find(id);
            if (mydata == null)
            {
                return HttpNotFound();
            }
            return View(mydata);
        }

        // POST: /Home/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Enum1,Enum2,FlagsEnum")] MyData mydata)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mydata).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mydata);
        }

        // GET: /Home/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyData mydata = db.MyDatas.Find(id);
            if (mydata == null)
            {
                return HttpNotFound();
            }
            return View(mydata);
        }

        // POST: /Home/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MyData mydata = db.MyDatas.Find(id);
            db.MyDatas.Remove(mydata);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
