using System;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateTicketModel
    {
        public Guid SeatId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectionId { get; set; }
    }
}