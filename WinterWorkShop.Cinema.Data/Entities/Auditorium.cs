using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WinterWorkShop.Cinema.Data
{
    [Table("auditorium")]
    public class Auditorium
    {
        [Key]
        public Guid Id { get; set; }
        
        [MaxLength(30)]
        public string Name { get; set; }
        
        public Guid CinemaId { get; set; }
        
        public virtual Cinema Cinema { get; set; }
        
        public ICollection<Seat> Seats { get; set; }
        
        public ICollection<Projection> Projections { get; set; }
    }
}
