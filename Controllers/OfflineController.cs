using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Recommendation_System.Controllers
{
    public class OfflineController : Controller
    {
        // GET: Offline
        public ActionResult Index()
        {
            return View();
        }
    }
}