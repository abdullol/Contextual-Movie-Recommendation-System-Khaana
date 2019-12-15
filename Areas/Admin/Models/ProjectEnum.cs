using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Movie_Recommendation_System.Areas.Admin.Models
{
    public class ProjectEnum
    {
        public enum TicketFee
        {
            [Description("Regular Basic")]
            Regular = 500,
            [Description("Gold")]
            Gold = 700,
            [Description("Platinum")]
            UltraPremium = 1000
        }
    }
}