using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IMovieParticipantRepository : IRepository<MovieParticipant>
    {
        Task<IEnumerable<MovieParticipant>> GetAllByMovieIdAsync(Guid movieId);
        Task<IEnumerable<MovieParticipant>> GetAllByParticipantIdAsync(Guid participantId);
    }
    public class MovieParticipantRepository : IMovieParticipantRepository
    {
        private CinemaContext _cinemaContext;

        public MovieParticipantRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public MovieParticipant Delete(object id)
        {
            MovieParticipant existing = _cinemaContext.MovieParticipants.Find(id);
            var result = _cinemaContext.MovieParticipants.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<MovieParticipant>> GetAllAsync()
        {
            var data = await _cinemaContext.MovieParticipants.ToListAsync();

            return data;
        }

        public async Task<MovieParticipant> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.MovieParticipants.FindAsync((Guid)id);

            return data;
        }

        public MovieParticipant Insert(MovieParticipant obj)
        {
            return _cinemaContext.MovieParticipants.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public MovieParticipant Update(MovieParticipant obj)
        {
            var updatedEntry = _cinemaContext.MovieParticipants.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }

        public async Task<IEnumerable<MovieParticipant>> GetAllByMovieIdAsync(Guid movieId)
        {
            List<MovieParticipant> allMovieParticipants = await _cinemaContext.MovieParticipants.ToListAsync();

            List<MovieParticipant> filteredMovieParticipants = allMovieParticipants.Where(x => x.MovieId == movieId).ToList();

            return filteredMovieParticipants;
        }

        public async Task<IEnumerable<MovieParticipant>> GetAllByParticipantIdAsync(Guid participantId)
        {
            List<MovieParticipant> allMovieParticipants = await _cinemaContext.MovieParticipants.ToListAsync();

            List<MovieParticipant> filteredMovieParticipants = allMovieParticipants.Where(x => x.ParticipantId == participantId).ToList();

            return filteredMovieParticipants;
        }
    }
}
