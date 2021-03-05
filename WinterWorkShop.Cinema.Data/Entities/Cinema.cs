using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WinterWorkShop.Cinema.Data
{
    [Table("cinema")]
    public class Cinema
    {
        [Key]
        public Guid Id { get; set; }
        
        [MaxLength(30)]
        public string Name { get; set; }
        
        public int AddressId { get; set; }
        
        public Address Address { get; set; }
        
        public ICollection<Auditorium> Auditoria { get; set; }
        
    }
}
