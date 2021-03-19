using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data.Enums;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class GetParticipantByIdModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = Messages.PARTICIPANT_FIRST_NAME_NOT_VALID)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = Messages.PARTICIPANT_LAST_NAME_NOT_VALID)]
        public string LastName { get; set; }

        [Required]
        public ParticipantType ParticipantType { get; set; }
    }
}
