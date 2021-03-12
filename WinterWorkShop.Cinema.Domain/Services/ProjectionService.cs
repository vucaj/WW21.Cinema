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
    public class ProjectionService : IProjectionService
    {
        private readonly IProjectionsRepository _projectionsRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly ICinemasRepository _cinemasRepository;
        private readonly IMoviesRepository _moviesRepository;

        public ProjectionService(IProjectionsRepository projectionsRepository, IAuditoriumsRepository auditoriumsRepository, ICinemasRepository cinemasRepository, IMoviesRepository moviesRepository)
        {
            _projectionsRepository = projectionsRepository;
            _auditoriumsRepository = auditoriumsRepository;
            _cinemasRepository = cinemasRepository;
            _moviesRepository = moviesRepository;
        }
        
        
        public async Task<IEnumerable<ProjectionDomainModel>> GetAllAsync()
        {
            var projections = await _projectionsRepository.GetAllAsync();

            return projections.Select(projection => new ProjectionDomainModel()
            {
                Id = projection.Id,
                AuditoriumId = projection.AuditoriumId,
                CinemaId = projection.Auditorium.CinemaId,
                DateTime = projection.DateTime,
                MovieId = projection.MovieId,
                TicketPrice = projection.TicketPrice
            });
        }

        public async Task<CreateProjectionResultModel> CreateProjection(ProjectionDomainModel domainModel)
        {
            var cinema = await _cinemasRepository.GetByIdAsync(domainModel.CinemaId);
            var movie = await _moviesRepository.GetByIdAsync(domainModel.MovieId);
            var auditorium = await _auditoriumsRepository.GetByIdAsync(domainModel.AuditoriumId);

            if (cinema == null || movie == null || auditorium == null)
            {
                return new CreateProjectionResultModel()
                {
                    ErrorMessage = Messages.PROJECTION_CREATION_ERROR,
                    IsSuccessful = false,
                    Projection = null
                };
            }

            if (domainModel.DateTime.CompareTo(DateTime.Now.AddDays(2)) < 0)
            {
                return new CreateProjectionResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_IN_PAST,
                    Projection = null
                };
            }

            Projection newProjection = new Projection()
            {
                Id = Guid.NewGuid(),
                Auditorium = auditorium,
                AuditoriumId = auditorium.Id,
                DateTime = domainModel.DateTime,
                Movie = movie,
                MovieId = movie.Id,
                TicketPrice = domainModel.TicketPrice,
                Tickets = new List<Ticket>()
            };

            Projection insertedProjection = _projectionsRepository.Insert(newProjection);

            if (insertedProjection == null)
            {
                return new CreateProjectionResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_CREATION_ERROR
                };
            }
            
            _projectionsRepository.Save();

            return new CreateProjectionResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Projection = new ProjectionDomainModel()
                {
                    Id = insertedProjection.Id,
                    AuditoriumId = insertedProjection.AuditoriumId,
                    CinemaId = insertedProjection.Auditorium.CinemaId,
                    DateTime = insertedProjection.DateTime,
                    MovieId = insertedProjection.MovieId,
                    TicketPrice = insertedProjection.TicketPrice
                }
            };
        }

        public Task<DeleteProjectionResultModel> DeleteProjection(ProjectionDomainModel domainModel)
        {
            throw new NotImplementedException();
        }
    }
}
