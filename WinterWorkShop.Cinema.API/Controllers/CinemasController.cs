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

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<CinemaDomainModel>> DeleteCinemaAsync([FromBody] DeleteCinemaModel deleteCinemaModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            CinemaDomainResultModel cinema = await _cinemaService.GetByCinemaId(new CinemaDomainModel
            {
                Id = deleteCinemaModel.Id
            });

            if (!cinema.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = cinema.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                
                return BadRequest(errorResponseModel);
            }

            CinemaDomainModel cinemaDomainModel = new CinemaDomainModel()
            {
                Id = cinema.Cinema.Id,
                AddressId = cinema.Cinema.AddressId,
                Name = cinema.Cinema.Name
            };

            DeleteCinemaResultModel resultModel = await _cinemaService.Delete(cinemaDomainModel);

            if (!resultModel.isSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = resultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                
                return BadRequest(errorResponseModel);
            }

            return Accepted("Cinema//" + resultModel.Cinema.Id, resultModel.Cinema);
        }
    }
}
