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

        [Display(Name = "Movie Showtime")]
        public string ShowTime { get; set; }

        [Display(Name = "Movie Showday")]
        public DateTime ShowDay { get; set; }
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