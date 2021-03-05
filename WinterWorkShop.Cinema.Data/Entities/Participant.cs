using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.Data
{
    [Table("participant")]
    public class Participant
    {
        [Key]
        public Guid Id { get; set; }
        
        [MaxLength(30)]
        public string FirstName { get; set; }
        
        [MaxLength(30)]
        public string LastName { get; set; }
        
        public ParticipantType ParticipantType { get; set; }
        
        public ICollection<MovieParticipant> MovieParticipants { get; set; }
    }
}