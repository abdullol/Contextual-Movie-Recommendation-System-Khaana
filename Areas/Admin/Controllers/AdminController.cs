using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Movie_Recommendation_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movie_Recommendation_System.Areas.Admin.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class AdminController : Controller
    {
        //[Authorize(Roles = "SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RegisterRole()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterRole(FormCollection form)
        {
            using (ApplicationDbContext _context = new ApplicationDbContext())
            {
                string roleName = form["RoleName"];

                var roleManager = new RoleManager<Role, int>(new RoleStore<Role, int, UserRole>(_context));

                if (!roleManager.RoleExists(roleName))
                {
                    var role = new Role(roleName);
                    roleManager.Create(role);
                }
            }
            return View("Index");
        }
        public ActionResult RoleAssignment()
        {
            using (ApplicationDbContext _context=new ApplicationDbContext())
            {
                //fetching Role for dropdown
                ViewBag.Role = _context.Roles.Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();
            }
            return View();
        }
        [HttpPost]
        public ActionResult RoleAssignment(FormCollection form)
        {
            using (ApplicationDbContext _context=new ApplicationDbContext())
            {
                string _userName = form["txtUserName"];
                string _roleName = form["RoleName"];

                ApplicationUser _user = _context.Users.Where(u => u.UserName.Equals(_userName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                var _userManager = new UserManager<ApplicationUser,int>(new UserStore<ApplicationUser, Role, int, UserLogin, UserRole, UserClaim>(_context));
                _userManager.AddToRole(_user.Id, _roleName);
            }

            return View("Index");
        }
    }
}