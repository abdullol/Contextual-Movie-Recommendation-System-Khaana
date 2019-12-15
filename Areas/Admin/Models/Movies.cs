using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Movie_Recommendation_System.Areas.Admin.Models
{
    public class Movies
    {
        public int Id { get; set; }

        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        [Display(Name = "Movie Poster File")]
        public string MoviePoster { get; set; }

        [Display(Name = "Movie Rating")]
        public decimal? MovieRating { get; set; }

        [Display(Name = "PG")]
        public string PGRating { get; set; }

        [Display(Name = "Movie Duration")]
        public string MovieDuration { get; set; }

        [Display(Name = "Release Date")]
        public string ReleaseDate { get; set; }

        [Display(Name = "Movie Director")]
        public string Director { get; set; }

        [Display(Name = "Movie Actor")]
        public string Actor { get; set; }

        [Display(Name = "Movie Writer")]
        public string Writer { get; set; }

        [Display(Name = "Movie Genre")]
        public string MovieGenre { get; set; }

        [Display(Name = "Description")]
        public string Story { get; set; }
        //one movie-many showtimes
        public ICollection<CinemaShowtimes> CinemaShowTimes { get; set; }
    }
}