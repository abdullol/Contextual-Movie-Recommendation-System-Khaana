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

        [Required(ErrorMessage = "Enter Movie Name")]
        [Display(Name = "Movie Name")]
        public string MovieName { get; set; }

        [Required(ErrorMessage = "Input Movie Poster, Correct URL")]
        [Display(Name = "Movie Poster File")]
        public string MoviePoster { get; set; }

        [Required(ErrorMessage ="Enter Movie Rating")]
        [Display(Name = "Movie Rating")]
        public decimal? MovieRating { get; set; }

        [Required(ErrorMessage ="Parent Guide Rating")]
        [Display(Name = "PG")]
        public string PGRating { get; set; }

        [Required(ErrorMessage ="Enter Movie Duration")]
        [Display(Name = "Movie Duration")]
        public string MovieDuration { get; set; }

        [Required(ErrorMessage ="Enter Release Date")]
        [Display(Name = "Release Date")]
        public string ReleaseDate { get; set; }

        [Required(ErrorMessage ="Enter Director Name")]
        [Display(Name = "Movie Director")]
        public string Director { get; set; }

        [Required(ErrorMessage = "Enter Actor Name")]
        [Display(Name = "Movie Actor")]
        public string Actor { get; set; }

        [Display(Name = "Movie Writer")]
        public string Writer { get; set; }

        [Required(ErrorMessage = "Enter Movie Genre")]
        [Display(Name = "Movie Genre")]
        public string MovieGenre { get; set; }

        [Required(ErrorMessage ="Enter Movie Description")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        public string Story { get; set; }
        //one movie-many showtimes
        public ICollection<CinemaShowtimes> CinemaShowTimes { get; set; }
    }
}