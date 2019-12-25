using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Movie_Recommendation_System.Areas.Admin.Models
{
    public class CinemaShowtimes
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Enter Show Timing")]
        [Display(Name = "Movie Showtime")]
        public string ShowTime { get; set; }

        [Required(ErrorMessage = "Enter Show Day Information")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [Display(Name = "Movie Showday")]
        public DateTime ShowDay { get; set; }

        [Required(ErrorMessage = "Enter Show Location")]
        public string Location { get; set; }

        //for db use
        [ForeignKey("CinemaInstance")]
        public int? CinemaId { get; set; }
        [ForeignKey("MoviesInstance")]
        public int? MoviesId { get; set; }

        //for operational use
        public Cinemas CinemaInstance { get; set; }
        public Movies MoviesInstance { get; set; }
    }
}