using Microsoft.AspNet.Identity;
using Movie_Recommendation_System.Areas.Admin.Models;
using Movie_Recommendation_System.Areas.Admin.ViewModels;
using Movie_Recommendation_System.Models;
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

        [Authorize]
        public ActionResult Index()
        {
            List<CinemaShowtimes> _showTimesList = new List<CinemaShowtimes>();
            //http://www.geoplugin.net/asp.gp?ip=xx.xx.xx.xx
            //http://www.geoplugin.net/json.gp?ip=xx.xx.xx.xx
            //https://jsonplaceholder.typicode.com/posts/1/comments

            //Reading the user's current location
            string _strTestUrl = String.Format("http://www.geoplugin.net/json.gp?ip=xx.xx.xx.xx");
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

            string _userLocation = objList.geoplugin_city;

            //makeing list of top rated movies by IMDB
            homePageVM obj = new homePageVM();
            List<Movies> Movies = new List<Movies>();
            var topIMDB = from st in db.Movies
                          orderby st.MovieRating descending
                          select st;
            obj.topIMDB = topIMDB.Take(11).ToList();

            //starting a batch file which contains algorithm
            string path = @"D:/Work/Movie_Recommendation_System/mrs/runAlgo.bat";
            ThreadPool.QueueUserWorkItem(o =>
            {
                var user = User.Identity.GetUserId();

                var proc = new Process
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
                proc.Start();

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
                //proc.WaitForExit();
            });

            for (int i = 0; i < 10; ++i)
            {
                Task.WaitAll(Task.Delay(2000));
            }
            if (Movies.Count == 0)
            {
                obj.recMovies = topIMDB.Take(11).ToList();
            }
            else
            {
                obj.recMovies = Movies;
            }

            var year = DateTime.Today.Year;
            var date = DateTime.Today.Day;
            var now = DateTime.Now;
            var hour = now.Hour;
            var minute = now.Minute;
            var sec = now.Second;
            var month = now.Month;
            var _currentDay = DateTime.Now.DayOfYear;

            //db.CinemaShowtimes.FirstOrDefault(t => t.MoviesId == item.Id &&
            //    t.ShowDay >= DateTime.Now && string.Compare(t.Location, _userLocation, true) == 0);
            //obj.inCinema.Add(_contextualItem);

            //1234User.. 1 / 1 / 2019 12:00:00 AM

            List<CinemaShowtimes> filteredData = new List<CinemaShowtimes>();

            foreach (var item in obj.recMovies)
            {
                var _contextualItem = db.CinemaShowtimes.Where(t => t.MoviesId == item.Id &&
                string.Compare(t.Location, _userLocation, true) == 0 &&
                t.ShowDay.Year > year ? t.ShowDay.Month >= month || t.ShowDay.Month <= month : t.ShowDay.Year == year ? t.ShowDay.Month >= month : t.ShowDay.Month == 0 &&
                 t.ShowDay.Day >= _currentDay).ToList();

                filteredData.AddRange(_contextualItem);
            }

            obj.inCinema = filteredData.ToList();

            if (obj.inCinema.Count <= 0)
            {
                return View(obj);
            }

            if (obj.inCinema.Count > 0 || obj.inCinema.Count < 5)
            {
                //obj.recMovies = obj.inCinema + obj.recMovies;
            }

            return View(obj);
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

        public ActionResult Movies(int? page)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return View(db.Movies.ToList().ToPagedList(page ?? 1, 10));
            }
        }

        public ActionResult GetTopIMDB(int? page)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return View(db.Movies.ToList().ToPagedList(page ?? 1, 10));
            }
        }

        public ActionResult GetShowtimes()
        {
            return View(db.CinemaShowtimes.Include("CinemaInstance").Include("MoviesInstance").ToList());
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
    }
}