using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data
{
    [Table("projection")]
    public class Projection
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public DateTime DateTime { get; set; }

        public Guid MovieId { get; set; }
        
        public virtual Movie Movie { get; set; }

        [Range(0, 2000)]
        public double TicketPrice { get; set; }
        
        public ICollection<Ticket> Tickets { get; set; }
        
        public Guid AuditoriumId { get; set; }
        public virtual Auditorium Auditorium { get; set; }
        
    }
}
