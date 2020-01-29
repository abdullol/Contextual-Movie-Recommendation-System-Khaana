using Movie_Recommendation_System.Areas.Admin.Models;
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
    public class CinemaShowtimesController : Controller
    {
        // GET: Admin/CinemaShowtimes
        public ActionResult Index()
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                return View(_context.CinemaShowtimes.Include("MoviesInstance").Include("CinemaInstance").ToList());
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            ViewBag.CinemaList = new SelectList(_context.Cinemas, "Id", "CinemaName");
            ViewBag.MovieList = new SelectList(_context.Movies, "Id", "MovieName");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CinemaShowtimes cinemaShowtime, int CinemaList, int MovieList)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            var movieShowtime = new CinemaShowtimes()
            {
                ShowDay = cinemaShowtime.ShowDay,
                ShowTime = cinemaShowtime.ShowTime,
                MoviesId = MovieList,
                CinemaId = CinemaList,
                Location=cinemaShowtime.Location
            };

            _context.CinemaShowtimes.Add(movieShowtime);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult Edit(int? id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CinemaShowtimes cinemas = _context.CinemaShowtimes.Find(id);

            CinemaShowtimes cinemaWithRefQuery = _context.CinemaShowtimes.Include("CinemaInstance").Include("MoviesInstance").SingleOrDefault(c => c.Id == cinemas.Id);

            if (cinemas == null)
            {
                return HttpNotFound();
            }

            return View(cinemaWithRefQuery);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CinemaShowtimes cinemaShowtimes)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            if (ModelState.IsValid)
            {
                _context.Entry(cinemaShowtimes).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cinemaShowtimes);
        }

        public ActionResult Details(int? id)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var showTime = _context.CinemaShowtimes.Include("CinemaInstance").Include("MoviesInstance").SingleOrDefault(c => c.Id == id);

                if (showTime == null)
                {
                    return HttpNotFound();
                }

                return View(showTime);
            }
        }

        public ActionResult Delete(int? id)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                CinemaShowtimes cinemas = _context.CinemaShowtimes.Find(id);

                CinemaShowtimes cinemaWithRefQuery = _context.CinemaShowtimes.Include("CinemaInstance").Include("MoviesInstance").SingleOrDefault(c => c.Id == cinemas.Id);

                if (cinemaWithRefQuery == null)
                {
                    return HttpNotFound();
                }
                return View(cinemaWithRefQuery);
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApplicationDbContext _context = new ApplicationDbContext();

            CinemaShowtimes showTimes = _context.CinemaShowtimes.Find(id);
            _context.CinemaShowtimes.Remove(showTimes);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}