using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data.Enums;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class MovieModel
    {
        [Required]
        [StringLength(50, ErrorMessage = Messages.MOVIE_PROPERTIE_TITLE_NOT_VALID)]
        public string Title { get; set; }
        
        [Required]
        [StringLength(300)]
        public string Description { get; set; }
        
        [Required]
        public Genre Genre { get; set; }
        
        
        [Required]
        [Range(1, 500)]
        public int Duration { get; set; }
        
        [Required]
        [Range(1.0, 10.0, ErrorMessage = Messages.MOVIE_PROPERTIE_RATING_NOT_VALID)]
        public double Rating { get; set; }
        
        [Required]
        [StringLength(30)]
        public string Distributer { get; set; }

        [Required]
        [Range(1900, 2050, ErrorMessage = Messages.MOVIE_PROPERTIE_YEAR_NOT_VALID)]
        public int Year{ get; set; }
        
        [Required]
        public bool IsActive { get; set; }
        
        [Required]
        [Range(0, 1000)]
        public int NumberOfOscars { get; set; }
        
    }
}
