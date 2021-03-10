using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class UpdateMovieParticipantModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid MovieId { get; set; }
        
        [Required]
        public Guid ParticipantId { get; set; }
    }
}
