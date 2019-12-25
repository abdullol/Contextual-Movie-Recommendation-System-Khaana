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

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Cinema Name")]
        public string CinemaName { get; set; }

        [Required]
        [Display(Name = "Movie Display Hall")]
        public int Halls { get; set; }

        [Required(ErrorMessage = "You must provide a phone number")]
        [Display(Name = "Contact #")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "You must provide Ticket Type")]
        [Display(Name = "Movie Ticket")]
        public TicketFee TicketFee { get; set; }

        [Required(ErrorMessage = "Ensure Seat Availability")]
        [Display(Name = "Seats Available")]
        public bool SeatsAvailable { get; set; }

        //one cinema-many showtimes
        public ICollection<CinemaShowtimes> CinemaShowTimes { get; set; }
    }
}