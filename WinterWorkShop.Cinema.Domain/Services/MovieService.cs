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
        public IEnumerable<MovieDomainModel> GetAllMovies(bool? isCurrent)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDomainModel> GetMovieByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDomainModel> AddMovie(MovieDomainModel newMovie)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDomainModel> DeleteMovie(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
