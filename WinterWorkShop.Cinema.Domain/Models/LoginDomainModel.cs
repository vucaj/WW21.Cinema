using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class LoginDomainModel
    {
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }
}