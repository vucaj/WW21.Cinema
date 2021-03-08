using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Enums;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly ICinemasRepository _cinemasRepository;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository, ICinemasRepository cinemasRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
            _cinemasRepository = cinemasRepository;
        }
        
        public async Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync()
        {
            var auditoria = await _auditoriumsRepository.GetAllAsync();

            return auditoria.Select(auditorium => new AuditoriumDomainModel
            {
                Id = auditorium.Id,
                CinemaId = auditorium.CinemaId,
                Name = auditorium.Name
            });
        }

        public async Task<CreateAuditoriumResultModel> CreateAuditorium(AuditoriumDomainModel domainModel, int numberOfRows, int numberOfSeats)
        {
            var cinema = await _cinemasRepository.GetByIdAsync(domainModel.CinemaId);
            if (cinema == null)
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_UNVALID_CINEMAID
                };
            }

            Auditorium newAuditorium = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = domainModel.Name,
                CinemaId = domainModel.CinemaId
            };

            newAuditorium.Seats = new List<Seat>();
            newAuditorium.Projections = new List<Projection>();

            for (int i = 1; i <= numberOfRows; i++)
            {
                for (int j = 1; j <= numberOfSeats; j++)
                {
                    if (numberOfRows >= 3)
                    {
                        Seat newSeat = new Seat()
                        {
                            Id = Guid.NewGuid(),
                            Row = i,
                            Number = j,
                            AuditoriumId = newAuditorium.Id
                        };
                        
                        if (i == 1)
                        {
                            newSeat.SeatType = SeatType.VIP;
                        }
                        else if (i < numberOfRows)
                        {
                            newSeat.SeatType = SeatType.REGULAR;
                        }
                        else
                        {
                            newSeat.SeatType = SeatType.LOVE_SEAT;
                        }
                        newAuditorium.Seats.Add(newSeat);
                    }
                    else
                    {
                        Seat newSeat = new Seat()
                        {
                            Id = Guid.NewGuid(),
                            Row = i,
                            Number = j,
                            SeatType = SeatType.REGULAR,
                            AuditoriumId = newAuditorium.Id
                        };
                        newAuditorium.Seats.Add(newSeat);
                    }
                }
            }

            Auditorium insertedAuditorium = _auditoriumsRepository.Insert(newAuditorium);

            if (insertedAuditorium == null)
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_CREATION_ERROR
                };
            }
            
            _auditoriumsRepository.Save();

            CreateAuditoriumResultModel resultModel = new CreateAuditoriumResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Auditorium =new AuditoriumDomainModel
                {
                    Id = insertedAuditorium.Id,
                    Name = insertedAuditorium.Name,
                    CinemaId = insertedAuditorium.CinemaId,
                }
            };

            return resultModel;
        }
    }
}
