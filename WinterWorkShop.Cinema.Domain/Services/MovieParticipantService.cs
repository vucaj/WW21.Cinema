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
    class MovieParticipantService : IMovieParticipantService
    {
        private readonly IMovieParticipantRepository _movieParticipantRepository;
        private readonly IParticipantRepository _participantRepository;

        public MovieParticipantService(IMovieParticipantRepository movieParticipantRepository, IParticipantRepository participantRepository)
        {
            _movieParticipantRepository = movieParticipantRepository;
            _participantRepository = participantRepository;
        }
        public async Task<CreateMovieParticipantResultModel> Create(MovieParticipantDomainModel domainModel)
        {
            MovieParticipant newMovieParticipant = new MovieParticipant
            {
                Id = Guid.NewGuid(),
                MovieId = domainModel.MovieId,
                ParticipantId = domainModel.ParticipantId
            };

            MovieParticipant insertedMovieParticipant = _movieParticipantRepository.Insert(newMovieParticipant);

            if (insertedMovieParticipant == null)
            {
                return new CreateMovieParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIEPARTICIPANT_SAVE_ERROR
                };
            }

            _movieParticipantRepository.Save();

            CreateMovieParticipantResultModel resultModel = new CreateMovieParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                MovieParticipant = new MovieParticipantDomainModel
                {
                    Id = insertedMovieParticipant.Id,
                    MovieId = insertedMovieParticipant.MovieId,
                    ParticipantId = insertedMovieParticipant.ParticipantId
                }
            };

            return resultModel;
        }

        public async Task<DeleteMovieParticipantResultModel> Delete(MovieParticipantDomainModel domainModel)
        {
            var movieParticipant = await _movieParticipantRepository.GetByIdAsync(domainModel.Id);
            if (movieParticipant == null)
            {
                return new DeleteMovieParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIEPARTICIPANT_NOT_FOUND
                };
            }

            _movieParticipantRepository.Delete(movieParticipant.Id);
            _movieParticipantRepository.Save();

            return new DeleteMovieParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null
            };
        }

        public async Task<IEnumerable<MovieParticipantDomainModel>> GetAllAsync()
        {
            var movieParticipant = await _movieParticipantRepository.GetAllAsync();
            return movieParticipant.Select(movieParticipants => new MovieParticipantDomainModel
            {
                Id = movieParticipants.Id,
                MovieId = movieParticipants.MovieId,
                ParticipantId = movieParticipants.ParticipantId
            });
        }

        public async Task<IEnumerable<CreateMovieParticipantResultModel>> GetAllByMovieIdAsync(MovieDomainModel domainModel)
        {
            var movieParticipants = await _movieParticipantRepository.GetAllByMovieIdAsync(domainModel.Id);

            if (movieParticipants == null)
            {
                return null;
            }

            return movieParticipants.Select(movieParticipant => new CreateMovieParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                MovieParticipant = new MovieParticipantDomainModel
                {
                    Id = movieParticipant.Id,
                    MovieId = movieParticipant.MovieId,
                    ParticipantId = movieParticipant.ParticipantId
                }
            });
        }

        public async Task<IEnumerable<CreateMovieParticipantResultModel>> GetAllByParticipantIdAsync(ParticipantDomainModel domainModel)
        {
            var movieParticipants = await _movieParticipantRepository.GetAllByParticipantIdAsync(domainModel.Id);

            if (movieParticipants == null)
            {
                return null;
            }

            return movieParticipants.Select(movieParticipant => new CreateMovieParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                MovieParticipant = new MovieParticipantDomainModel
                {
                    Id = movieParticipant.Id,
                    MovieId = movieParticipant.MovieId,
                    ParticipantId = movieParticipant.ParticipantId
                }
            });
        }

        public async Task<CreateMovieParticipantResultModel> GetByMovieParticipantId(MovieParticipantDomainModel domainModel)
        {
            var movieParticipant = await _movieParticipantRepository.GetByIdAsync(domainModel.Id);
            if (movieParticipant == null)
            {
                return new CreateMovieParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIEPARTICIPANT_NOT_FOUND
                };
            }
            return new CreateMovieParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                MovieParticipant = new MovieParticipantDomainModel
                {
                    Id = movieParticipant.Id,
                    MovieId = movieParticipant.MovieId,
                    ParticipantId = movieParticipant.ParticipantId
                }
            };
        }

        // todo: implementirati metodu GetByMovieIdAsync

        public async Task<DeleteParticipantResultModel> GetByParticipantIdAsync(Guid id)
        {
            var participant = await _participantRepository.GetByIdAsync(id);

            if(participant == null)
            {
                return new DeleteParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PARTICIPANT_NOT_FOUND
                };
            }

            return new DeleteParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null
            };
        }

        public async Task<UpdateMovieParticipantResultModel> Update(MovieParticipantDomainModel domainModel)
        {
            var movieParticipant = await _movieParticipantRepository.GetByIdAsync(domainModel.Id);

            if (movieParticipant == null)
            {
                return new UpdateMovieParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIEPARTICIPANT_NOT_FOUND
                };
            }

            var updatedMovieParticipant = _movieParticipantRepository.Update(movieParticipant);

            if (updatedMovieParticipant == null)
            {
                return new UpdateMovieParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIEPARTICIPANT_UPDATE_ERROR
                };
            }

            _movieParticipantRepository.Save();

            UpdateMovieParticipantResultModel resultModel = new UpdateMovieParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null
            };

            return resultModel;
        }
    }
}
