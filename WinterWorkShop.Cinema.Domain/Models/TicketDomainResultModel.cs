namespace WinterWorkShop.Cinema.Domain.Models
{
    public class TicketDomainResultModel
    {
        public bool IsSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public TicketDomainModel Ticket { get; set; }
    }
}