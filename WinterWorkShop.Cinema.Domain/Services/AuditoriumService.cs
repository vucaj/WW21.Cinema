using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumRepository _auditoriumsRepository;

        public AuditoriumService(IAuditoriumRepository auditoriumRepository)
        {
            _auditoriumsRepository = auditoriumRepository;
        }
        
        public async Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync()
        {
            var auditoria = await _auditoriumsRepository.GetAllAsync();

            return auditoria.Select(auditorium => new AuditoriumDomainModel
            {
                Id = auditorium.Id,
                CinemaId = auditorium.CinemaId,
                Name = auditorium.Name
                /*SeatsList = auditorium.Seats.Select(seat => new SeatDomainModel
                {
                   Id = seat.Id,
                   AuditoriumId = seat.AuditoriumId,
                   Number = seat.Number,
                   Row = seat.Row,
                   SeatType = seat.SeatType
                }).ToList(),
                
                ProjectionsList = auditorium.Projections.Select(projection => new ProjectionDomainModel
                {
                    Id = projection.Id,
                    DateTime = projection.DateTime,
                    MovieId = projection.MovieId,
                    AuditoriumId = projection.AuditoriumId,
                    CinemaId = projection.CinemaId,
                    TicketPrice = projection.TicketPrice
                }).ToList()*/
            });
        }

        public Task<CreateAuditoriumResultModel> CreateAuditorium(AuditoriumDomainModel domainModel, int numberOfRows, int numberOfSeats)
        {
            throw new NotImplementedException();
        }
    }
}
