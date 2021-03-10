using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class SeatDomainModel
    {
        public Guid Id { get; set; }

        public Guid AuditoriumId { get; set; }

        [Range(0, 150)]
        public int Row { get; set; }

        [Range(0, 150)]
        public int Number { get; set; }

        public SeatType SeatType { get; set; }
    }
}
