using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Movie_Recommendation_System.Areas.Admin.Models;
using Movie_Recommendation_System.Models;

namespace Movie_Recommendation_System.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class CinemasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Cinemas.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinemas cinemas = db.Cinemas.Find(id);
            if (cinemas == null)
            {
                return HttpNotFound();
            }
            return View(cinemas);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Cinemas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CinemaName,Halls,ContactNumber,TicketFee,isGoogleMapApiActive,SeatsAvailable,StaticLocation,latitude,longitude")] Cinemas cinemas)
        {
            if (ModelState.IsValid)
            {
                db.Cinemas.Add(cinemas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cinemas);
        }

        // GET: Admin/Cinemas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cinemas cinemas = db.Cinemas.Find(id);
            
            if (cinemas == null)
            {
                return HttpNotFound();
            }
            return View(cinemas);
        }

        // POST: Admin/Cinemas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CinemaName,Halls,ContactNumber,TicketFee,isGoogleMapApiActive,SeatsAvailable,StaticLocation,latitude,longitude")] Cinemas cinemas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cinemas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinemas);
        }

        // GET: Admin/Cinemas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinemas cinemas = db.Cinemas.Find(id);
            if (cinemas == null)
            {
                return HttpNotFound();
            }
            return View(cinemas);
        }

        // POST: Admin/Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cinemas cinemas = db.Cinemas.Find(id);
            db.Cinemas.Remove(cinemas);
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
