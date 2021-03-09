using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.API.Models
{
    public class UpdateParticipantModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        public ParticipantType ParticipantType { get; set; }
    }
}
