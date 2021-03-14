using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface ITicketRepostory: IRepository<Ticket>{}
    
    
    public class TicketRespository: ITicketRepostory
    {
        private CinemaContext _cinemaContext;

        public TicketRespository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }
        
        public Ticket Delete(object id)
        {
            Ticket existing = _cinemaContext.Tickets.Find(id);
            var result = _cinemaContext.Tickets.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            var data = await _cinemaContext.Tickets.ToListAsync();

            return data;
        }

        public async Task<Ticket> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.Tickets.FindAsync(id);

            return data;
        }

        public Ticket Insert(Ticket obj)
        {
            return _cinemaContext.Tickets.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Ticket Update(Ticket obj)
        {
            var updatedEntry = _cinemaContext.Tickets.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
    }
}