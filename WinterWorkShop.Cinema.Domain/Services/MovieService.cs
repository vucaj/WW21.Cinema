using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMoviesRepository _moviesRepository;

        public MovieService(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }
        
        public IEnumerable<MovieDomainModel> GetAllMovies(bool? isCurrent)
        {
            var data = _moviesRepository.GetCurrentMovies();

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();

            foreach (var item in data)
            {
                MovieDomainModel movie = new MovieDomainModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Genre = item.Genre,
                    Duration = item.Duration,
                    Distributer = item.Distributer,
                    IsActive = item.IsActive,
                    NumberOfOscars = item.NumberOfOscars,
                    Rating = item.Rating,
                    Year = item.Year
                };
                
                result.Add(movie);
            }

            return result;
        }

        public async Task<IEnumerable<MovieDomainModel>> GetAllMovies()
        {
            var data = await _moviesRepository.GetAllAsync();

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();

            foreach (var item in data)
            {
                MovieDomainModel movie = new MovieDomainModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Genre = item.Genre,
                    Duration = item.Duration,
                    Distributer = item.Distributer,
                    IsActive = item.IsActive,
                    NumberOfOscars = item.NumberOfOscars,
                    Rating = item.Rating,
                    Year = item.Year
                };
                
                result.Add(movie);
            }

            return result;
        }

        public async Task<IEnumerable<MovieDomainModel>> GetTop10Async()
        {
            var data =  _moviesRepository.GetTop10();

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();

            foreach (var item in data)
            {
                MovieDomainModel movie = new MovieDomainModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Genre = item.Genre,
                    Duration = item.Duration,
                    Distributer = item.Distributer,
                    IsActive = item.IsActive,
                    NumberOfOscars = item.NumberOfOscars,
                    Rating = item.Rating,
                    Year = item.Year
                };
                
                result.Add(movie);
            }

            return result;
            
        }

        public async Task<IEnumerable<MovieDomainModel>> GetTop10ByYearAsync(int year)
        {
            var data =  _moviesRepository.GetTop10ByYear(year);

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();

            foreach (var item in data)
            {
                MovieDomainModel movie = new MovieDomainModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Description = item.Description,
                    Genre = item.Genre,
                    Duration = item.Duration,
                    Distributer = item.Distributer,
                    IsActive = item.IsActive,
                    NumberOfOscars = item.NumberOfOscars,
                    Rating = item.Rating,
                    Year = item.Year
                };
                
                result.Add(movie);
            }

            return result;
        }

        public async Task<MovieDomainModel> GetMovieByIdAsync(Guid id)
        {
            Movie data = await _moviesRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }

            return new MovieDomainModel
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                Genre = data.Genre,
                Duration = data.Duration,
                Distributer = data.Distributer,
                IsActive = data.IsActive,
                NumberOfOscars = data.NumberOfOscars,
                Rating = data.Rating,
                Year = data.Year
            };
        }

        public async Task<MovieDomainModel> AddMovie(MovieDomainModel newMovie)
        {
            Movie movieToCreate = new Movie()
            {
                Id = Guid.NewGuid(),
                Title = newMovie.Title,
                Description = newMovie.Description,
                Genre = newMovie.Genre,
                Duration = newMovie.Duration,
                Distributer = newMovie.Distributer,
                IsActive = newMovie.IsActive,
                NumberOfOscars = newMovie.NumberOfOscars,
                Rating = newMovie.Rating,
                Year = newMovie.Year
            };

            Movie data = _moviesRepository.Insert(movieToCreate);
            if (data == null)
            {
                return null;
            }
            
            _moviesRepository.Save();

            return new MovieDomainModel
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                Genre = data.Genre,
                Duration = data.Duration,
                Distributer = data.Distributer,
                IsActive = data.IsActive,
                NumberOfOscars = data.NumberOfOscars,
                Rating = data.Rating,
                Year = data.Year
            };
        }

        public async Task<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie)
        {
            Movie movie = await _moviesRepository.GetByIdAsync(updateMovie.Id);

            if (movie == null)
            {
                return null;
            }

            Movie movieToUpdate = new Movie()
            {
                Id = movie.Id,
                Title = updateMovie.Title,
                Description = updateMovie.Description,
                Genre = updateMovie.Genre,
                Duration = updateMovie.Duration,
                Distributer = updateMovie.Distributer,
                IsActive = updateMovie.IsActive,
                NumberOfOscars = updateMovie.NumberOfOscars,
                Rating = updateMovie.Rating,
                Year = updateMovie.Year
            };

            Movie data = _moviesRepository.Update(movieToUpdate);

            if (data == null)
            {
                return null;
            }
            
            _moviesRepository.Save();

            return new MovieDomainModel()
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                Genre = data.Genre,
                Duration = data.Duration,
                Distributer = data.Distributer,
                IsActive = data.IsActive,
                NumberOfOscars = data.NumberOfOscars,
                Rating = data.Rating,
                Year = data.Year
            };
        }

        public async Task<MovieDomainModel> DeleteMovie(Guid id)
        {
            Movie movie = await _moviesRepository.GetByIdAsync(id);
            
            if (movie == null)
            {
                return null;
            }
            
            Movie data = _moviesRepository.Delete(id);

            if (data == null)
            {
                return null;
            }
            
            _moviesRepository.Save();

            return new MovieDomainModel
            {
                Id = data.Id,
                Title = data.Title,
                Description = data.Description,
                Genre = data.Genre,
                Duration = data.Duration,
                Distributer = data.Distributer,
                IsActive = data.IsActive,
                NumberOfOscars = data.NumberOfOscars,
                Rating = data.Rating,
                Year = data.Year
            };
        }
    }
}
