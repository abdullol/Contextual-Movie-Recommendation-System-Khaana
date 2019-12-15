using Movie_Recommendation_System.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Recommendation_System.Areas.Admin.ViewModels
{
    public class CinemaMoviesVM
    {
        public int Id { get; set; }
        public IEnumerable<Cinemas> Cinemas { get; set; }
        //foreign key of cinema
        [Display(Name = "Cinema Name")]
        public int CinemaId { get; set; }
        public IEnumerable<Movies> Movies { get; set; }
        //foreign key of movie
        public int MovieId { get; set; }
        public CinemaShowtimes CinemaShowtimes { get; set; }
    }
}