namespace WinterWorkShop.Cinema.Domain.Models
{
    public class UserDomainResultModel
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public UserDomainModel user { get; set; }
    }
}