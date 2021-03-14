﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;

        public UserService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        
        public async Task<IEnumerable<UserDomainModel>> GetAllAsync()
        {
            var users = await _usersRepository.GetAllAsync();

            return users.Select(user => new UserDomainModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = "",
                BonusPoints = user.BonusPoints,
                Role = user.Role,
                UserName = user.UserName
            });
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
