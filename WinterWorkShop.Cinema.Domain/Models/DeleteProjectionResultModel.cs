namespace WinterWorkShop.Cinema.Domain.Models
{
    public class DeleteProjectionResultModel
    {
        public ProjectionDomainModel Projection { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccessful { get; set; }
    }
}