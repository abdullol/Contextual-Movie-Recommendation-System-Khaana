using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Movie_Recommendation_System.Areas.Admin.ViewModels;
using Movie_Recommendation_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Recommendation_System.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult UserProfile()
        {
            UserProfileVM _userProfileVM = new UserProfileVM();

            var _currentUserId = User.Identity.GetUserId().ToString();

            //_userProfileVM.ApplicationUsers = _context.Users.FirstOrDefault(u => u.Id == _currentUserId);
            _userProfileVM.Watchlists = _context.Watchlists.Where(w => w.userId.Equals(_currentUserId)).ToList();

            return View(_userProfileVM);
        }
    }
}