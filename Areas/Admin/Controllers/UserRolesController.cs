using Movie_Recommendation_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Movie_Recommendation_System.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Admin/UserRoles
        public ActionResult Index()
        {
            var _listOfUser = _context.Users.ToList();

            return View(_listOfUser);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser _appUser = _context.Users.Find(id);

            return View(_appUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(applicationUser).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser _appUser = _context.Users.Find(id);

            if (_appUser==null)
            {
                return HttpNotFound();
            }

            return View(_appUser);
        }

        public ActionResult Delete(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser _appUser = _context.Users.Find(id);

            if (_appUser==null)
            {
                return HttpNotFound();
            }

            return View(_appUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationUser _appUser = _context.Users.Find(id);
            _context.Users.Remove(_appUser);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}