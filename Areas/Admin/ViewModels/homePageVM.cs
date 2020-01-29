using Movie_Recommendation_System.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Movie_Recommendation_System.Enums.CityEnum;

namespace Movie_Recommendation_System.Areas.Admin.ViewModels
{
    public class homePageVM
    {
        public List<Movies> recMovies { get; set; }
        public List<Movies> topIMDB { get; set; }
        public List<CinemaShowtimes> inCinema { get; set; }
        public CityCollection CityCollection { get; set; }
    }
}