using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class AuditoriumsController : ControllerBase
    {
        private readonly IAuditoriumService _auditoriumService;

        public AuditoriumsController(IAuditoriumService auditoriumService)
        {
            _auditoriumService = auditoriumService;
        }

        /// Gets all auditoriums
        
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<AuditoriumDomainModel>>> GetAllAsync()
        {
            IEnumerable<AuditoriumDomainModel> auditoriumDomainModels;

            auditoriumDomainModels = await _auditoriumService.GetAllAsync();

            if (auditoriumDomainModels == null)
            {
                auditoriumDomainModels = new List<AuditoriumDomainModel>();
            }

            return Ok(auditoriumDomainModels); 
        }
        
        /// Adds a new auditorium

        //[Authorize(Roles = "admin")]
        
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<AuditoriumDomainModel>> CreateAuditoriumAsync ([FromBody] CreateAuditoriumModel createAuditoriumModel) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuditoriumDomainModel auditoriumDomainModel = new AuditoriumDomainModel
            {
                CinemaId = createAuditoriumModel.CinemaId,
                Name = createAuditoriumModel.Name
                
            };

            CreateAuditoriumResultModel createAuditoriumResultModel;

            try 
            {
                createAuditoriumResultModel = await _auditoriumService.CreateAuditorium(auditoriumDomainModel, createAuditoriumModel.SeatRows, createAuditoriumModel.NumberOfSeats);
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

            if (!createAuditoriumResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = createAuditoriumResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            
            return Created("auditoriums//" + createAuditoriumResultModel.Auditorium.Id, createAuditoriumResultModel);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteAuditorium([FromBody] DeleteAuditoriumModel deleteAuditoriumModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AuditoriumDomainModel auditoriumDomainModel = new AuditoriumDomainModel
            {
                Id = deleteAuditoriumModel.AuditoriumId,
                CinemaId = deleteAuditoriumModel.CinemaId
            };

            DeleteAuditoriumResultModel deleteAuditoriumResultModel;

            try
            {
                deleteAuditoriumResultModel = await _auditoriumService.DeleteAuditorium(auditoriumDomainModel);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            if (!deleteAuditoriumResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel()
                {
                    ErrorMessage = deleteAuditoriumResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            return Ok("Deleted auditorium: " + auditoriumDomainModel.Id);

        }
        
    }
}