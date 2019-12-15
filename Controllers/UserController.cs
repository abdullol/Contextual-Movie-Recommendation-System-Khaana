using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
            var userId = int.Parse(User.Identity.GetUserId());

            ApplicationUser _currentUser = _context.Users.FirstOrDefault(u => u.Id == userId);

            var currentUser = new ApplicationUser
            {
                Id = _currentUser.Id,
                Email = _currentUser.Email,
                UserName = _currentUser.UserName
            };


            return View(currentUser);
        }
    }
}