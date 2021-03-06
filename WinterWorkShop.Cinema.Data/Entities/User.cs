using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.Data
{
    [Table("user")]
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [MaxLength(30)]
        public string FirstName { get; set; }

        [MaxLength(30)]
        public string LastName { get; set; }

        [Column("userName")]
        [MinLength(5)]
        [MaxLength(20)]
        [Required] 
        public string UserName { get; set; }

        [MinLength(8)]
        [MaxLength(20)]
        [Required]
        public string Password { get; set; }

        public UserRole Role { get; set; }
        
        [Range(0, Int32.MaxValue)]
        public int BonusPoints { get; set; }
        
        public ICollection<Ticket> Tickets { get; set; }
    }
}
