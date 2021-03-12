using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly ICinemasRepository _cinemasRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly ISeatsRepository _seatsRepository;

        public CinemaService(ICinemasRepository cinemasRepository, IAuditoriumsRepository auditoriumsRepository, ISeatsRepository seatsRepository)
        {
            _cinemasRepository = cinemasRepository;
            _auditoriumsRepository = auditoriumsRepository;
            _seatsRepository = seatsRepository;
        }
        
        public async Task<IEnumerable<CinemaDomainModel>> GetAllAsync()
        {
            var cinemas = await _cinemasRepository.GetAllAsync();

            return cinemas.Select(cinema => new CinemaDomainModel
            {
                Id = cinema.Id,
                Name = cinema.Name,
                AddressId = cinema.AddressId
            });
        }

        public async Task<CinemaDomainResultModel> GetByCinemaId(CinemaDomainModel cinemaDomainModel)
        {
            var cinema = await _cinemasRepository.GetByIdAsync(cinemaDomainModel.Id);

            if (cinema == null)
            {
                return new CinemaDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.CINEMA_NOT_FOUND,
                    Cinema = null
                };
            }

            return new CinemaDomainResultModel()
            {
                IsSuccessful = true,
                Cinema = new CinemaDomainModel()
                {
                    Id = cinema.Id,
                    AddressId = cinema.AddressId,
                    Name = cinema.Name
                }
            };
        }

        public async Task<CreateCinemaResultModel> Create(CinemaDomainModel domainModel)
        {
            Data.Cinema newCinema = new Data.Cinema
            {
                Id = Guid.NewGuid(),
                Name = domainModel.Name,
                AddressId = domainModel.AddressId
            };

            newCinema.Auditoria = new List<Auditorium>();

            Data.Cinema insertedCinema = _cinemasRepository.Insert(newCinema);

            if (insertedCinema == null)
            {
                return new CreateCinemaResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.CINEMA_SAVE_ERROR
                };
            }
            
            _cinemasRepository.Save();

            CreateCinemaResultModel resultModel = new CreateCinemaResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Cinema = new CinemaDomainModel
                {
                    Id = insertedCinema.Id,
                    Name = insertedCinema.Name,
                    AddressId = insertedCinema.AddressId
                }
            };

            return resultModel;
        }

        public async Task<DeleteCinemaResultModel> Delete(CinemaDomainModel domainModel)
        {
            var cinema = await _cinemasRepository.GetByIdAsync(domainModel.Id);
            if (cinema == null)
            {
                return new DeleteCinemaResultModel
                {
                    isSuccessful = false,
                    ErrorMessage = Messages.CINEMA_NOT_FOUND
                };
            }

            var auditoria = await _auditoriumsRepository.GetByCinemaId(cinema.Id);

            foreach (var auditorium in auditoria)
            {
                var seats = await _seatsRepository.GetAllByAuditoriumIdAsync(auditorium.Id);

                foreach (var seat in seats)
                {
                    _seatsRepository.Delete(seat.Id);
                }
                _seatsRepository.Save();

                _auditoriumsRepository.Delete(auditorium.Id);
                _auditoriumsRepository.Save();
            }

            _cinemasRepository.Delete(cinema.Id);
            _cinemasRepository.Save();

            return new DeleteCinemaResultModel
            {
                isSuccessful = true,
                Cinema = new CinemaDomainModel
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    AddressId = cinema.AddressId
                },
                ErrorMessage = null
            };

        }
    }

}
