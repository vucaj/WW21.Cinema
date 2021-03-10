using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class MovieDomainModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        public Genre Genre { get; set; }
        
        public int Duration { get; set; }
        
        public String Distributer { get; set; }
        
        public bool IsActive { get; set; }
        
        public int NumberOfOscars { get; set; }
        
        public double Rating { get; set; }
        
        public int Year { get; set; }
    }
}
