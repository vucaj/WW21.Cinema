using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public CinemaService(ICinemasRepository cinemasRepository)
        {
            _cinemasRepository = cinemasRepository;
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
    }

}
