using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateCinemaResultModel
    {
        public CinemaDomainModel Cinema { get; set; }
        
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        
    }
}