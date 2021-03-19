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
                CinemaId = projection.CinemaId,
                DateTime = projection.DateTime,
                MovieId = projection.MovieId,
                TicketPrice = projection.TicketPrice,
                AuditoriumName = projection.Auditorium.Name,
                CinemaName = projection.Cinema.Name,
                MovieRating = projection.Movie.Rating,
                MovieTitle = projection.Movie.Title
            });
        }

        public async Task<ProjectionDomainResultModel> GetById(Guid id)
        {
            var projection = await _projectionsRepository.GetByIdAsync(id);

            if (projection == null)
            {
                return new ProjectionDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_NOT_FOUND,
                    Projection = null
                };
            }

            return new ProjectionDomainResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Projection = new ProjectionDomainModel()
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    CinemaId = projection.CinemaId,
                    DateTime = projection.DateTime,
                    MovieId = projection.MovieId,
                    TicketPrice = projection.TicketPrice
                }
            };
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

            // TODO: proveriti da li u tom auditoriumu postoji projekcija koja se poklapa sa novom projekcijom
            
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
                CinemaId = domainModel.CinemaId,
                AuditoriumId = auditorium.Id,
                DateTime = domainModel.DateTime,
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
                    CinemaId = insertedProjection.CinemaId,
                    DateTime = insertedProjection.DateTime,
                    MovieId = insertedProjection.MovieId,
                    TicketPrice = insertedProjection.TicketPrice
                }
            };
        }

        public async Task<DeleteProjectionResultModel> DeleteProjection(ProjectionDomainModel domainModel)
        {
            var projection = await _projectionsRepository.GetByIdAsync(domainModel.Id);

            //TODO: proveriti da li postoji ticket za projekciju.
            
            if (projection == null)
            {
                return new DeleteProjectionResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_NOT_FOUND
                };
            }

            _projectionsRepository.Delete(projection.Id);
            _projectionsRepository.Save();

            return new DeleteProjectionResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Projection = new ProjectionDomainModel()
                {
                    Id = projection.Id,
                    AuditoriumId = projection.AuditoriumId,
                    CinemaId = projection.CinemaId,
                    DateTime = projection.DateTime,
                    MovieId = projection.MovieId,
                    TicketPrice = projection.TicketPrice
                }
            };
        }

        public async Task<IEnumerable<ProjectionDomainModel>> GetFutureProjections()
        {
            var projections = await _projectionsRepository.GetFutureProjections();

            return projections.Select(projection => new ProjectionDomainModel()
            {
                Id = projection.Id,
                AuditoriumId = projection.AuditoriumId,
                CinemaId = projection.CinemaId,
                DateTime = projection.DateTime,
                MovieId = projection.MovieId,
                TicketPrice = projection.TicketPrice, 
                MovieTitle = projection.Movie.Title
            });
            
        }
    }
}
