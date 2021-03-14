using System.Collections.Generic;
using System.Threading.Tasks;
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
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Ticket>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Ticket> GetByIdAsync(object id)
        {
            throw new System.NotImplementedException();
        }

        public Ticket Insert(Ticket obj)
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public Ticket Update(Ticket obj)
        {
            throw new System.NotImplementedException();
        }
    }
}