using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<UserDomainModel>>> GetAsync()
        {
            IEnumerable<UserDomainModel> userDomainModels;

            userDomainModels = await _userService.GetAllAsync();

            if (userDomainModels == null)
            {
                userDomainModels = new List<UserDomainModel>();
            }

            return Ok(userDomainModels);
        }

        /// <summary>
        /// Gets User by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("id/{id}")]
        public async Task<ActionResult<UserDomainModel>> GetbyIdAsync(Guid id)
        {
            var model = await _userService.GetUserByIdAsync(id);

            if (!model.IsSuccessful)
            {
                return NotFound(Messages.USER_NOT_FOUND);
            }

            return Ok(model);
        }

        // <summary>
        /// Gets User by UserName
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("username/{username}")]
        public async Task<ActionResult<UserDomainModel>> GetbyUserNameAsync(string username)
        {
            var model =  _userService.GetUserByUserName(username);

            if (!model.IsSuccessful)
            {
                return NotFound(Messages.USER_NOT_FOUND);
            }

            return Ok(model);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserDomaniModel createUserDomaniModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserDomainModel domainModel = new UserDomainModel()
            {
                Id = Guid.NewGuid(),
                BonusPoints = createUserDomaniModel.BonusPoints,
                FirstName = createUserDomaniModel.FirstName,
                LastName = createUserDomaniModel.LastName,
                Password = createUserDomaniModel.Password,
                Role = createUserDomaniModel.Role,
                UserName = createUserDomaniModel.Username
            };

            UserDomainResultModel createUser;

            try
            {
                createUser = await _userService.CreateUser(domainModel);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel()
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            if (!createUser.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel()
                {
                    ErrorMessage = createUser.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            return Created("User//" + createUser.user.Id, createUser.user);
        }
    }
}
