using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class TicketService: ITicketService
    {
        private readonly ITicketRepostory _ticketRepostory;

        public TicketService(ITicketRepostory ticketRepostory)
        {
            _ticketRepostory = ticketRepostory;
        }
        
        public async Task<IEnumerable<TicketDomainModel>> GetAllAsync()
        {
            var tickets = await _ticketRepostory.GetAllAsync();

            return tickets.Select(ticket => new TicketDomainModel()
            {
                Id = ticket.Id,
                ProjectionId = ticket.ProjectionId,
                SeatId = ticket.SeatId,
                UserId = ticket.UserId
            });
        }

        public async Task<IEnumerable<TicketDomainModel>> GetByProjectionId(Guid projectionId)
        {
            var tickets = await _ticketRepostory.GetByProjectionId(projectionId);

            return tickets.Select(ticket => new TicketDomainModel()
            {
                Id = ticket.Id,
                ProjectionId = ticket.ProjectionId,
                SeatId = ticket.SeatId,
                UserId = ticket.UserId
            });
        }

        public async Task<IEnumerable<TicketDomainModel>> GetByUserId(Guid userId)
        {
            var tickets = await _ticketRepostory.GetByUserId(userId);

            return tickets.Select(ticket => new TicketDomainModel()
            {
                Id = ticket.Id,
                ProjectionId = ticket.ProjectionId,
                SeatId = ticket.SeatId,
                UserId = ticket.UserId
            });
        }

        public async Task<TicketDomainResultModel> GetById(Guid id)
        {
            var ticket = await _ticketRepostory.GetByIdAsync(id);

            if (ticket == null)
            {
                return new TicketDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.TICKET_NOT_FOUND,
                    Ticket = null
                };
            }

            return new TicketDomainResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Ticket = new TicketDomainModel()
                {
                    Id = ticket.Id,
                    ProjectionId = ticket.ProjectionId,
                    SeatId = ticket.SeatId,
                    UserId = ticket.UserId
                }
            };
        }
    }
}