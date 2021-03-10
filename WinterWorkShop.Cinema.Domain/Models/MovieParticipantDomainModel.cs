using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class MovieParticipantDomainModel
    {
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }

        public Guid ParticipantId { get; set; }
    }
}
