namespace WinterWorkShop.Cinema.Domain.Models
{
    public class CinemaDomainResultModel
    {
        public bool IsSuccessful { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public CinemaDomainModel Cinema { get; set; }
    }
}