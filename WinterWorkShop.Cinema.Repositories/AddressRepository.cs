using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IAddressRepository : IRepository<Address>
    {
        
    }
    public class AddressRepository : IAddressRepository
    {
        private CinemaContext _cinemaContext;
        public AddressRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }
        public Address Delete(object id)
        {
            Address existing = _cinemaContext.Addresses.Find(id);
            var result = _cinemaContext.Addresses.Remove(existing);
            return result.Entity;
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            var data = await _cinemaContext.Addresses.ToListAsync();
            return data;
        }

        public async Task<Address> GetByIdAsync(object id)
        {
            return await _cinemaContext.Addresses.FindAsync(id);
        }

        public Address Insert(Address obj)
        {
            var data = _cinemaContext.Addresses.Add(obj).Entity;
            return data;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Address Update(Address obj)
        {
            var data = _cinemaContext.Addresses.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;
            return data.Entity;
        }
    }
}
