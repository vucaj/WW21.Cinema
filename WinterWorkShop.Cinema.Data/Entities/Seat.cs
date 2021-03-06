using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.Data
{
    [Table("seat")]
    public class Seat
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [Range(0,150)]
        public int Row { get; set; }

        [Range(0,150)]
        public int Number { get; set; }
        
        public Guid AuditoriumId { get; set; }
        
        public virtual Auditorium Auditorium { get; set; }

        public SeatType SeatType { get; set; }
    }
}
