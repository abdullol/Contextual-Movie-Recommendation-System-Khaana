using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Movie_Recommendation_System.Areas.Admin.Models.ProjectEnum;

namespace Movie_Recommendation_System.Areas.Admin.Models
{
    public class Cinemas
    {
        public int Id { get; set; }

        [Display(Name = "Cinema Name")]
        public string CinemaName { get; set; }

        [Display(Name = "Movie Display Hall")]
        public int Halls { get; set; }

        [Display(Name = "Contact #")]
        [Required(ErrorMessage = "You must provide a phone number")]
        [DataType(DataType.PhoneNumber)]
        public string ContactNumber { get; set; }

        [Display(Name = "Movie Ticket")]
        public TicketFee TicketFee { get; set; }

        [Display(Name = "Seats Available")]
        public bool SeatsAvailable { get; set; }

        //one cinema-many showtimes
        public ICollection<CinemaShowtimes> CinemaShowTimes { get; set; }
    }
}