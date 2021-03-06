using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.Data
{
    [Table("movie")]
    public class Movie
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [MaxLength(30)]
        public string Title { get; set; }
        
        [MaxLength(300)]
        public string Description { get; set; }
        
        public Genre Genre { get; set; }
        
        [Range(1, 500)]
        public int Duration { get; set; }
        
        [Range(1.0, 10.0)]
        public double? Rating { get; set; }
        
        [MaxLength(30)]
        public string Distributer { get; set; }
        
        [Range(1900, 2050)]
        public int Year { get; set; }
        
        public bool IsActive { get; set; }
        
        [Range(0, 1000)]
        public int NumberOfOscars { get; set; }
        
        public ICollection<MovieParticipant> MovieParticipants { get; set; }
        
        public ICollection<Projection> Projections { get; set; }
    }
}
