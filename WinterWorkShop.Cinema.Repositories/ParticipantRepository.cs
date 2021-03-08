using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IParticipantRepository : IRepository<Participant> 
    {
/*        Task<IEnumerable<Participant>> GetByFirstName(string firstName);
        Task<IEnumerable<Participant>> GetByLastName(string lastName);*/
    }
    public class ParticipantRepository : IParticipantRepository
    {
        private CinemaContext _cinemaContext;

        public ParticipantRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Participant Delete(object id)
        {
            Participant existing = _cinemaContext.Participants.Find(id);
            var result = _cinemaContext.Participants.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Participant>> GetAllAsync()
        {
            var data = await _cinemaContext.Participants.ToListAsync();

            return data;
        }

/*        public async Task<IEnumerable<Participant>> GetByFirstName(string firstName)
        {
            var data = await _cinemaContext.Participants.Where(x => x.FirstName.Equals(firstName)).ToListAsync();
            return data;
        }*/

        public async Task<Participant> GetByIdAsync(object id)
        {
            return await _cinemaContext.Participants.FindAsync(id);
        }

/*        public async Task<IEnumerable<Participant>> GetByLastName(string lastName)
        {
            var data = await _cinemaContext.Participants.Where(x => x.LastName.Equals(lastName)).ToListAsync();
            return data;
        }*/

        public Participant Insert(Participant obj)
        {
            var data = _cinemaContext.Participants.Add(obj).Entity;

            return data;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Participant Update(Participant obj)
        {
            var updatedEntry = _cinemaContext.Participants.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
    }
}
