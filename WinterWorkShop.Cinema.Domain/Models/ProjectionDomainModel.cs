using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class ProjectionDomainModel
    {
        public Guid Id { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public Guid MovieId { get; set; }
        
        public string MovieTitle { get; set; }
        
        public double MovieRating { get; set; }
        
        public Guid AuditoriumId { get; set; }
        
        public string AuditoriumName { get; set; }
        
        public Guid CinemaId { get; set; }
        
        public string CinemaName { get; set; }
        
        public double TicketPrice { get; set; }
        
    }
}
