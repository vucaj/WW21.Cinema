using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CinemasController : ControllerBase
    {
        private readonly ICinemaService _cinemaService;

        public CinemasController(ICinemaService cinemaService)
        {
            _cinemaService = cinemaService;
        }


        /// Gets all cinemas

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<CinemaDomainModel>>> GetAsync()
        {
            IEnumerable<CinemaDomainModel> cinemaDomainModels;

            cinemaDomainModels = await _cinemaService.GetAllAsync();

            if (cinemaDomainModels == null)
            {
                cinemaDomainModels = new List<CinemaDomainModel>();
            }

            return Ok(cinemaDomainModels);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> CreateCinemaAsync([FromBody] CreateCinemaModel createCinemaModel)
        {
            //TODO: provertiti unetu adresu

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CinemaDomainModel cinemaDomainModel = new CinemaDomainModel
            {
                Id = Guid.NewGuid(),
                Name = createCinemaModel.Name,
                AddressId = createCinemaModel.AddressId
            };

            CinemaDomainModel createCinema;

            try
            {
                createCinema = await _cinemaService.Create(cinemaDomainModel);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (createCinema == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.CINEMA_CREATION_ERROR,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Created("Cinema//" + createCinema.Id, createCinema);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delte(Guid id)
        {
            CinemaDomainModel deletedCinema;
            try
            {
                deletedCinema = await _cinemaService.Delete(id);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            if (deletedCinema == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.CINEMA_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }
            return Accepted("cinema//" + deletedCinema.Id, deletedCinema);
        }
    }
}
