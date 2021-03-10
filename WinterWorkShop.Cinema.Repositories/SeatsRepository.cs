using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Enums;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface ISeatsRepository : IRepository<Seat>
    {
        Task<IEnumerable<Seat>> GetAllByAuditoriumIdAsync(Guid auditoriumId);
        Task<IEnumerable<Seat>> GetByAllSeatTypeAsync(SeatType seatType);
    //        get all by audit id ----
    //        get all free
    //        get all reserved
    //        get by type  -------
    //        
    }
    public class SeatsRepository : ISeatsRepository
    {
        private CinemaContext _cinemaContext;

        public SeatsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Seat Delete(object id)
        {
            Seat existing = _cinemaContext.Seats.Find(id);
            var result = _cinemaContext.Seats.Remove(existing).Entity;

            return result;
        }

        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            var data = await _cinemaContext.Seats.ToListAsync();

            return data;
        }

        public async Task<Seat> GetByIdAsync(object id)
        {
            return await _cinemaContext.Seats.FindAsync((Guid)id);
        }

        public Seat Insert(Seat obj)
        {
            var data = _cinemaContext.Seats.Add(obj).Entity;

            return data;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Seat Update(Seat obj)
        {
            var updatedEntry = _cinemaContext.Seats.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }

        public async Task<IEnumerable<Seat>> GetAllByAuditoriumIdAsync(Guid auditoriumId)
        {
            List<Seat> allSeats = await _cinemaContext.Seats.ToListAsync();

            List<Seat> filteredSeats = allSeats.Where(x => x.AuditoriumId == auditoriumId).ToList();

            return filteredSeats;
        }

        public async Task<IEnumerable<Seat>> GetByAllSeatTypeAsync(SeatType seatType)
        {
            List<Seat> allSeats = await _cinemaContext.Seats.ToListAsync();

            List<Seat> filteredSeats = allSeats.Where(x => x.SeatType == seatType).ToList();

            return filteredSeats;
        }
    }
}
