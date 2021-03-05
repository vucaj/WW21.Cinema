using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinterWorkShop.Cinema.Data
{
    [Table("movieParticipant")]
    public class MovieParticipant
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }
        
        public Movie Movie { get; set; }
        
        public Guid ParticipantId { get; set; }
        
        public Participant Participant { get; set; }
    }
}