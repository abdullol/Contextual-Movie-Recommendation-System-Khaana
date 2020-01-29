using Movie_Recommendation_System.Areas.Admin.Models;
using Movie_Recommendation_System.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Recommendation_System.ViewModels
{
    public class CinemaShowtimeVM
    {
        public CityEnum CityCollection { get; set; }
        public List<CinemaShowtimes> UserLocationBased { get; set; }
        public List<CinemaShowtimes> DropdownBased { get; set; }
    }
}