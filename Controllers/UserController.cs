using Microsoft.AspNet.Identity;
using Movie_Recommendation_System.Areas.Admin.ViewModels;
using Movie_Recommendation_System.Models;
using PagedList;
using System.Linq;
using System.Web.Mvc;

namespace Movie_Recommendation_System.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public ActionResult UserProfile()
        {
            UserProfileVM _userProfileVM = new UserProfileVM();

            var _currentUserId = int.Parse(User.Identity.GetUserId());
            _userProfileVM.ApplicationUsers = _context.Users.Find(_currentUserId);
            _userProfileVM.Watchlists = _context.Watchlists.Include("MovieInstance").Where(w => w.userId.Equals(_currentUserId.ToString())).ToList();

            return View(_userProfileVM);
        }
    }
}