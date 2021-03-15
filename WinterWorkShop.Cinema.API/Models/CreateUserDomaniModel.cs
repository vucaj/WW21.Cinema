using System.ComponentModel.DataAnnotations;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateUserDomaniModel
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public UserRole Role { get; set; }

        public int BonusPoints { get; set; }
        
        
    }
}