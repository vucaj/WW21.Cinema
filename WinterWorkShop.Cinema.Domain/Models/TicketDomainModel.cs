using System;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class TicketDomainModel
    {
        public Guid Id { get; set; }
        public Guid SeatId { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectionId { get; set; }
    }
}