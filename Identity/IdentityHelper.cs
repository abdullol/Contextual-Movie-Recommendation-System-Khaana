using Movie_Recommendation_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Movie_Recommendation_System.Identity
{
    public static class IdentityHelper
    {
        #region Properties

        private const string SUPER_USER_ROLE = "SuperAdmin";
        private const string USER_ROLE = "User";

        #endregion

        #region Methods
        public static bool IsSuperAdminUser(string userName)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var userId = db.Users.FirstOrDefault(_ => _.UserName == userName)?.Id;
                var user = db.Users.Include("Roles").FirstOrDefault(_ => _.Id == userId);
                var role = db.Roles.FirstOrDefault(_ => _.Name == SUPER_USER_ROLE);

                if (user != null && role != null)
                    return user.Roles.FirstOrDefault(_ => _.RoleId == role.Id) != null;

                return false;
            }
        }

        public static bool IsSuperAdminUser(Guid userId)
        {
            //using (ApplicationDbContext db = new ApplicationDbContext())
            //{
            //    var user = db.Users.Include("Roles").FirstOrDefault(_ => _.Id == userId);
            //    var role = db.Roles.FirstOrDefault(_ => _.Name == SUPER_USER_ROLE);

            //    if (user != null && role != null)
            //        return user.Roles.FirstOrDefault(_ => _.RoleId == role.Id) != null;

            //    return false;
            //}

            return false;
        }
        #endregion

    }
}