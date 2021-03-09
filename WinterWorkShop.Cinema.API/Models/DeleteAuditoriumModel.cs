using System;
using System.ComponentModel.DataAnnotations;

namespace WinterWorkShop.Cinema.API.Models
{
    public class DeleteAuditoriumModel
    {
        [Required]
        public Guid AuditoriumId { get; set; }
        
        [Required]
        public Guid CinemaId { get; set; }
    }
}