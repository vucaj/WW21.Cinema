using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class UserService : IUserService
    {
        public Task<IEnumerable<UserDomainModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDomainModel> GetUserByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDomainModel> GetUserByUserName(string username)
        {
            throw new NotImplementedException();
        }
    }
}
