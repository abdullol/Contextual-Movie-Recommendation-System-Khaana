using Movie_Recommendation_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Recommendation_System.Areas.Admin.ViewModels
{
    public class UserProfileVM
    {
        public ApplicationUser ApplicationUsers { get; set; }
        public List<Watchlist> Watchlists { get; set; }
    }
}