using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class AuditoriumDomainModel
    {
        public Guid Id { get; set; }

        public Guid  CinemaId { get; set; }

        public string Name { get; set; }
        
    }
}
