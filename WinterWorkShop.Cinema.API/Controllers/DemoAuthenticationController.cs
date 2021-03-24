using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.TokenServiceExtensions;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [OpenApiIgnore]
    public class DemoAuthenticationController : ControllerBase    
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public DemoAuthenticationController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        // NOT FOR PRODUCTION USE!!!
        // you will need a robust auth implementation for production
        // i.e. try IdentityServer4
        [Route("/get-token/{username}")]
        public ActionResult<LoginDomainModel> GenerateToken(string username)
        {
            var user = _userService.GetUserByUserName(username);

            if (!user.IsSuccessful)
            {
                return BadRequest();
            }

            var role = user.user.Role;
            var name = user.user.FirstName;
            
            var jwt = JwtTokenGenerator
                .Generate(name, role, _configuration["Tokens:Issuer"], _configuration["Tokens:Key"]);

            LoginDomainModel loginDomainModel = new LoginDomainModel()
            {
                Token = jwt,
                Role = role
            };
            
            
            return Ok(loginDomainModel);
        }
    }
}
