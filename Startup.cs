using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Movie_Recommendation_System.Models;
using Owin;
using System.Web.Http;
namespace Movie_Recommendation_System
{
    public partial class Startup
    {
        [Authorize(Roles = "SuperAdmin")]
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateUserAndRoles();
        }
        public void CreateUserAndRoles()
        {
            ApplicationDbContext _context = new ApplicationDbContext();
            
            //RoleManagr id actual class to work with but canNot work with RoleStore
            //RoleManager contains high level classes such as CreateUser
            //RoleStore is reponsible for storage in Db

            var roleManager = new RoleManager<Role, int>(new RoleStore<Role, int, UserRole>(_context));
            var userManager = new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser, Role, int,
        UserLogin, UserRole, UserClaim>(_context));

            if (!roleManager.RoleExists("SuperAdmin"))
            {
                //creating super-admin role
                var role = new Role("SuperAdmin");
                roleManager.Create(role);

                //create default-user
                var user = new ApplicationUser();
                user.UserName = "mustafa@admin.com";
                user.Email = "mustafa@admin.com";

                string pwd = "1234User..";

                var newUser = userManager.Create(user, pwd);

                if (newUser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "SuperAdmin");
                }
            }
        }
    }
}
