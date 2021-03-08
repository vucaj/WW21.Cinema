using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinterWorkShop.Cinema.Data
{
    [Table("ticket")]
    public class Ticket
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public Guid SeatId { get; set; }
        
        public virtual Seat Seat { get; set; }
        
        public Guid UserId { get; set; }
        
        public virtual User User { get; set; }
        
        public Guid ProjectionId { get; set; }
        public virtual Projection Projection { get; set; }
    }
}