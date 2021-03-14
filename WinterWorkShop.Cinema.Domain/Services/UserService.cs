using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
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

        public async Task<UserDomainResultModel> GetUserByIdAsync(Guid id)
        {
            var user = await _usersRepository.GetByIdAsync(id);

            if (user == null)
            {
                return new UserDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_NOT_FOUND,
                    user = null
                };
            }

            return new UserDomainResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                user = new UserDomainModel()
                {
                    Id = user.Id,
                    BonusPoints = user.BonusPoints,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = "",
                    UserName = user.UserName,
                    Role = user.Role
                }
            };
        }

        public async Task<UserDomainResultModel> GetUserByUserName(string username)
        {
            var user = _usersRepository.GetByUserName(username);

            if (user == null)
            {
                return new UserDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_NOT_FOUND,
                    user = null
                };
            }

            return new UserDomainResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                user = new UserDomainModel()
                {
                    Id = user.Id,
                    BonusPoints = user.BonusPoints,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Password = "",
                    UserName = user.UserName,
                    Role = user.Role
                }
            };
        }

        public async Task<UserDomainResultModel> CreateUser(UserDomainModel domainModel)
        {
            User user = _usersRepository.GetByUserName(domainModel.UserName);
            if (user != null)
            {
                return new UserDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_USERNAME_ALREADY_EXIST,
                    user = null
                };
            }
            
            User newUser = new User()
            {
                Id = domainModel.Id,
                BonusPoints = 0,
                FirstName = domainModel.FirstName,
                LastName = domainModel.LastName,
                Password = domainModel.Password,
                Role = domainModel.Role,
                UserName = domainModel.UserName,
                Tickets = new List<Ticket>()
            };

            User insertedUser = _usersRepository.Insert(newUser);

            if (insertedUser == null)
            {
                return new UserDomainResultModel()
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_CREATE_ERROR,
                    user = null
                };
            }
            
            _usersRepository.Save();

            return new UserDomainResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                user = new UserDomainModel()
                {
                    Id = insertedUser.Id,
                    BonusPoints = insertedUser.BonusPoints,
                    FirstName = insertedUser.UserName,
                    LastName = insertedUser.LastName,
                    Password = insertedUser.Password,
                    Role = insertedUser.Role,
                    UserName = insertedUser.UserName,
                }
            };

        }
    }
}
