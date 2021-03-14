namespace WinterWorkShop.Cinema.Domain.Models
{
    public class ProjectionDomainResultModel
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public ProjectionDomainModel Projection { get; set; }
    }
}