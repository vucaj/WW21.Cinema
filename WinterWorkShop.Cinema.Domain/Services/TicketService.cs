using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class TicketService: ITicketService
    {
        private readonly ITicketRepostory _ticketRepostory;
        private readonly IProjectionsRepository _projectionsRepository;
        private readonly ISeatsRepository _seatsRepository;
        private readonly IUsersRepository _usersRepository;

        public TicketService(ITicketRepostory ticketRepostory, IProjectionsRepository projectionsRepository, ISeatsRepository seatsRepository, IUsersRepository usersRepository)
        {
            _ticketRepostory = ticketRepostory;
            _projectionsRepository = projectionsRepository;
            _seatsRepository = seatsRepository;
            _usersRepository = usersRepository;
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

        public async Task<CreateTicketDomainResultModel> GetById(Guid id)
        {
            var ticket = await _ticketRepostory.GetByIdAsync(id);

            if (ticket == null)
            {
                return new CreateTicketDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.TICKET_NOT_FOUND,
                    Ticket = null
                };
            }

            return new CreateTicketDomainResultModel()
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

        public async Task<CreateTicketDomainResultModel> CreateTicket(TicketDomainModel domainModel)
        {
            var projection = await _projectionsRepository.GetByIdAsync(domainModel.ProjectionId);
            var seat = await _seatsRepository.GetByIdAsync(domainModel.SeatId);
            var user = await _usersRepository.GetByIdAsync(domainModel.UserId);

            if (projection == null || seat == null || user == null)
            {
                return new CreateTicketDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.TICKET_CREATE_ERROR,
                    Ticket = null
                };
            }
            
            Ticket newTicket = new Ticket
            {
                Id = Guid.NewGuid(),
                ProjectionId = projection.Id,
                SeatId = seat.Id,
                UserId = user.Id,
            };

            Ticket insertedTicket = _ticketRepostory.Insert(newTicket);

            if (insertedTicket == null)
            {
                return new CreateTicketDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.TICKET_SAVE_ERROR,
                    Ticket = null
                };
            }
            
            _ticketRepostory.Save();

            return new CreateTicketDomainResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Ticket = new TicketDomainModel()
                {
                    Id = insertedTicket.Id,
                    ProjectionId = insertedTicket.ProjectionId,
                    SeatId = insertedTicket.SeatId,
                    UserId = insertedTicket.UserId
                }
            };
        }
    }
}