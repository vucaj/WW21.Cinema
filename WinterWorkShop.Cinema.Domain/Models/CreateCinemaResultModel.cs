using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateCinemaResultModel
    {
        public bool IsSuccessful { get; set; }

        public string ErrorMessage { get; set; }

        public CinemaDomainModel Cinema { get; set; }
    }
}