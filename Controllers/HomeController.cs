using Microsoft.AspNet.Identity;
using Movie_Recommendation_System.Areas.Admin.Models;
using Movie_Recommendation_System.Areas.Admin.ViewModels;
using Movie_Recommendation_System.Models;
using Movie_Recommendation_System.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Movie_Recommendation_System.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<CinemaShowtimes> _showTimesList = new List<CinemaShowtimes>();



            string _userLocation = GetUserCurrentLocation();

            //makeing list of top rated movies by IMDB
            homePageVM obj = new homePageVM();
            List<Movies> Movies = new List<Movies>();
            var topIMDB = from st in db.Movies
                          orderby st.MovieRating descending
                          select st;
            obj.topIMDB = topIMDB.Take(11).ToList();

            //starting a batch file which contains algorithm
            var user = User.Identity.GetUserId();
            Process proc = GetProcessSettings();
            proc.Start();
            proc.WaitForExit();

            string line = string.Empty;
            //reading file till end
            while (!proc.StandardOutput.EndOfStream)
            {
                line = proc.StandardOutput.ReadLine();
                int con;
                if (Int32.TryParse(line, out con))
                {
                    Movies.Add(db.Movies.Find(con));
                }
            }

            if (Movies.Count == 0 || !Request.IsAuthenticated)
            {
                Random rand = new Random();
                for (int i = 0; i < 11; i++)
                {
                    int j = rand.Next(1, 1056);
                    Movies.Add(db.Movies.Find(j));
                }
                obj.recMovies = Movies;
            }
            else
            {
                obj.recMovies = Movies;
            }

            int year, month, _currentDay;
            GetContextualFeature(out year, out month, out _currentDay);

            List<CinemaShowtimes> filteredData = new List<CinemaShowtimes>();
            if (Request.IsAuthenticated)
            {
                foreach (var item in obj.recMovies)
                {
                    var _contextualItem = db.CinemaShowtimes.Where(t => t.MoviesId == item.Id &&
                    string.Compare(t.Location, _userLocation, true) == 0).ToList();

                    filteredData.AddRange(_contextualItem);
                }

                obj.inCinema = filteredData.ToList();

            }
            else
            {
                obj.inCinema = db.CinemaShowtimes.Include("MoviesInstance").Take(11).ToList();
            }

            if (obj.inCinema.Count == 0)
            {
                obj.inCinema = db.CinemaShowtimes.Include("MoviesInstance").Take(11).ToList();
                return View(obj);
            }

            if (obj.inCinema.Count > 0 && obj.inCinema.Count < 5)
            {
                //obj.recMovies = obj.inCinema + obj.recMovies;
            }

            return View(obj);
        }

        private Process GetProcessSettings()
        {
            string path = @"D:/Work/Movie_Recommendation_System/mrs/runAlgo.bat";

            return new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = path,
                    Arguments = User.Identity.GetUserId(),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
        }

        private static void GetContextualFeature(out int year, out int month, out int _currentDay)
        {
            year = DateTime.Today.Year;
            var now = DateTime.Now;
            month = now.Month;
            _currentDay = DateTime.Now.DayOfYear;
        }

        private static string GetUserCurrentLocation()
        {
            //Reading the user's current location

            string _strTestUrl = String.Format("https://api.ipdata.co/72.255.54.212?api-key=459e1ae1099e5a44b75e19c115415e36b4cb76d81f7f68d51eb7e658");
            //string _strTestUrl = String.Format("http://www.geoplugin.net/json.gp?ip=xx.xx.xx.xx");
            WebRequest _requestObjGet = WebRequest.Create(_strTestUrl);
            _requestObjGet.Method = "GET";
            HttpWebResponse _responseObjGet = null;
            _responseObjGet = (HttpWebResponse)_requestObjGet.GetResponse();

            string _strResultTest = null;

            //reading the Stream from Response object
            using (Stream stream = _responseObjGet.GetResponseStream())
            {
                StreamReader sr = new StreamReader(stream);
                _strResultTest = sr.ReadToEnd();
                sr.Close();
            }

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            UserContext objList = (UserContext)serializer.Deserialize(_strResultTest, typeof(UserContext));

            string _userLocation = objList.city;
            return _userLocation;
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public JsonResult WatchList(string id, string url)
        {
            int autoId = 0;
            int.TryParse(id, out autoId);

            if (!User.Identity.IsAuthenticated)
            {
                return Json("Not Authenticated");
            }

            if (autoId.Equals(0))
            {
                return Json("<br />Sorry! Record doesn't exists.");
            }

            var identityUser = User.Identity.GetUserId();

            var isIt = db.Watchlists.Where(u => u.movieId == autoId &&
              u.userId.Equals(identityUser)).FirstOrDefault();

            if (isIt != null)
            {
                HttpCookie httpCookie = new HttpCookie(url, "true");
                Response.Cookies.Add(httpCookie);
                return Json("<br/>Thanks You Have Already Voted");
            }

            var movie = db.Movies.Where(m => m.Id == autoId).FirstOrDefault();

            db.Entry(movie).State = EntityState.Modified;
            db.SaveChanges();

            var watchListUpdated = new Watchlist()
            {
                userId = User.Identity.GetUserId(),
                movieId = autoId
            };

            db.Watchlists.Add(watchListUpdated);
            db.SaveChanges();

            return Json("good");
        }

        public JsonResult SendRating(string r, string id, string url)
        {
            int autoId = 0;
            short rating = 0;
            var timestampNow = DateTime.Now;
            short.TryParse(r, out rating);
            int.TryParse(id, out autoId);

            if (!User.Identity.IsAuthenticated)
            {
                return Json("Not Authenticated");
            }

            if (autoId.Equals(0))
            {
                return Json("<br />Sorry! Record doesn't exists.");
            }
            var identityUser = User.Identity.GetUserId();
            //check if the user has already been signed in
            var isIt = db.Ratings.Where(u => u.movieId == autoId &&
            u.userId.Equals(identityUser)).FirstOrDefault();

            if (isIt != null)
            {
                HttpCookie httpCookie = new HttpCookie(url, "true");
                Response.Cookies.Add(httpCookie);
                return Json("<br/>Thanks You Have Already Voted");
            }


            var movie = db.Movies.Where(m => m.Id == autoId).FirstOrDefault();

            db.Entry(movie).State = EntityState.Modified;
            db.SaveChanges();

            var ratingTableUpdated = new Ratings()
            {
                userId = User.Identity.GetUserId(),
                movieId = autoId,
                rating = rating,
                timestamp = timestampNow
            };

            db.Ratings.Add(ratingTableUpdated);
            db.SaveChanges();

            ////Adding data in csv file
            string arg = identityUser + "," + id + "," + r + "," + "1260759144" + Environment.NewLine;

            using (StreamWriter sw = new StreamWriter(@"D:\Work\Movie_Recommendation_System\mrs\ratings.csv", true))
            {
                sw.Write(arg);
                sw.Close();
                sw.Dispose();
            }

            return Json("<br/>You Rated " + r + " star(s), Thanks");
        }
        public ActionResult Search(string Search) {
            return RedirectToAction("Movies", new
            {
                Sch = Search
            });
        }

        public ActionResult Movies(int? page,string Sch)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (Sch == null)
                {
                    return View(db.Movies.ToList().ToPagedList(page ?? 1, 10));

                }
                else {
                    List<Movies> movs = new List<Movies>();
                    movs = db.Movies.Where(r => r.MovieName.Contains(Sch)).ToList();
                    return View(movs.ToPagedList(page ?? 1, 10));
                }

            }
        }

        public ActionResult GetTopIMDB(int? page)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return View(db.Movies.ToList().ToPagedList(page ?? 1, 10));
            }
        }

        //public ActionResult showTimesGet(string city)
        //{
        //    int year, month, _currentDay;
        //    GetContextualFeature(out year, out month, out _currentDay);
        //    if (city == null)
        //    {
        //        string _userLocation = GetUserCurrentLocation();

        //        List<CinemaShowtimes> cs = db.CinemaShowtimes.Where(t =>t.MoviesId.tol && string.Compare(t.Location, _userLocation, true) == 0 &&
        //          t.ShowDay.Year > year ? t.ShowDay.Month >= month || t.ShowDay.Month <= month : t.ShowDay.Year == year ? t.ShowDay.Month >= month : t.ShowDay.Month == 0 &&
        //          t.ShowDay.Day >= _currentDay).Include("MoviesInstance").Include("CinemaInstance").ToList();
        //    }
        //    else
        //    {
        //        List<CinemaShowtimes> cs = db.CinemaShowtimes.Where(t => string.Compare(t.Location, city, true) == 0 &&
        //          t.ShowDay.Year > year ? t.ShowDay.Month >= month || t.ShowDay.Month <= month : t.ShowDay.Year == year ? t.ShowDay.Month >= month : t.ShowDay.Month == 0 &&
        //          t.ShowDay.Day >= _currentDay).Include("MoviesInstance").Include("CinemaInstance").ToList();
        //    }
        //    return Content("ShowTimes Returned");
        //}
        public ActionResult GetShowtimesList(string cs)
        {
            return RedirectToAction("GetShowtimes", new
            {
                city = cs
            });
        }
        public ActionResult GetShowtimes(string city)
        {
            int year, month, _currentDay;
            GetContextualFeature(out year, out month, out _currentDay);
            List<CinemaShowtimes> cs = new List<CinemaShowtimes>();
            if (city != null)
            {
                cs = db.CinemaShowtimes.Where(t => string.Compare(t.Location, city, true) == 0).Include("MoviesInstance").Include("CinemaInstance").ToList();
            }
            else
            {
                string _userLocation = GetUserCurrentLocation();

                cs = db.CinemaShowtimes.Where(t => string.Compare(t.Location.ToLower(), _userLocation.ToLower(), true) == 0).Include("MoviesInstance").Include("CinemaInstance").ToList();

                

            }
            return View(cs.ToList());
        }

        public ActionResult GetMovie(int id)
        {
            using (var _context = new ApplicationDbContext())
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movies movies = db.Movies.Find(id);

                return View(movies);
            }
        }

        public ActionResult GetWatchList(int? page)
        {
            var _currentUserId = User.Identity.GetUserId();
            var _watchList = db.Watchlists.Where(u => u.userId.Equals(_currentUserId)).Include("MovieInstance").ToList().ToPagedList(page ?? 1, 10);
            return View(_watchList);
        }

        public ActionResult DeleteWatchlist(int id)
        {
            Watchlist _watchlist = db.Watchlists.Find(id);
            db.Watchlists.Remove(_watchlist);
            db.SaveChanges();
            return RedirectToAction("GetWatchList");
        }
    }
}