using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateProjectionModel
    {
        [Required]
        public Guid AuditoriumId { get; set; }
        
        [Required]
        public Guid CinemaId { get; set; }
        
        [Required]
        public String DateTime { get; set; }
        
        [Required]
        public Guid MovieId { get; set; }
        
        [Required]
        public double TicketPrice { get; set; }
    }
}
