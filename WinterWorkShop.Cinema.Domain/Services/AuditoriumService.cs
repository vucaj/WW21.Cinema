using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ISeatsRepository _seatsRepository;
        private readonly IProjectionsRepository _projectionsRepository;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository, ICinemasRepository cinemasRepository, ISeatsRepository seatsRepository, IProjectionsRepository projectionsRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
            _cinemasRepository = cinemasRepository;
            _seatsRepository = seatsRepository;
            _projectionsRepository = projectionsRepository;
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
                    ErrorMessage = Messages.AUDITORIUM_INVALID_CINEMAID
                };
            }

            Auditorium newAuditorium = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = domainModel.Name,
                CinemaId = domainModel.CinemaId
            };

            if (cinema.Name.Equals(newAuditorium.Name))
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_SAME_NAME
                };
            }

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

        public async Task<DeleteAuditoriumResultModel> DeleteAuditorium(AuditoriumDomainModel domainModel)
        {
            var cinema = await _cinemasRepository.GetByIdAsync(domainModel.CinemaId);
            if (cinema == null)
            {
                return new DeleteAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_INVALID_CINEMAID
                };
            }

            var auditorium = await _auditoriumsRepository.GetByIdAsync(domainModel.Id);
            if (auditorium == null)
            {
                return new DeleteAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_NOT_FOUND
                };
            }

            var seats = await _seatsRepository.GetAllByAuditoriumIdAsync(auditorium.Id);

            foreach (var seat in seats)
            {
                _seatsRepository.Delete(seat.Id);
            }

            //TODO: i projekcije treba gettovati prvo
            /*foreach (var projection in auditorium.Projections)
            {
                _projectionsRepository.Delete(projection.Id);
            }*/

            _auditoriumsRepository.Delete(auditorium.Id);
            
            _projectionsRepository.Save();
            _seatsRepository.Save();
            _auditoriumsRepository.Save();

            DeleteAuditoriumResultModel resultModel = new DeleteAuditoriumResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null
            };

            return resultModel;
        }
    }
}
