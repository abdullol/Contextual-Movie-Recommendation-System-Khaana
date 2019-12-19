using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Movie_Recommendation_System.Models
{
    public class UserContext
    {
        public int id { get; set; }
        public string geoplugin_city { get; set; }
        public string geoplugin_region { get; set; }
    }
}