using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AuditoriumDomainModel>> GetById(Guid id)
        {
            var auditorium = await _auditoriumService.FindByAuditoriumId(new AuditoriumDomainModel()
            {
                Id = id,
            });

            if (auditorium == null)
            {
                return BadRequest();
            }

            return Ok(auditorium);
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
            
            return Created("auditoriums//" + createAuditoriumResultModel.Auditorium.Id, createAuditoriumResultModel.Auditorium);
        }

        /*[HttpPost]
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

        }*/

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateAuditorium(
            [FromBody] UpdateAuditoriumModel updateAuditoriumModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var auditorium = await _auditoriumService.FindByAuditoriumId(new AuditoriumDomainModel
            {
                Id = updateAuditoriumModel.Id
            });

            auditorium.Name = updateAuditoriumModel.Name;
            UpdateAuditoriumResultModel updateAuditoriumResultModel = await _auditoriumService.UpdateAuditorium(new AuditoriumDomainModel
            {
                Id = auditorium.Id,
                Name = auditorium.Name,
                CinemaId = auditorium.CinemaId
            });

            if (!updateAuditoriumResultModel.IsSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            AuditoriumDomainModel deletedAudit;
            try
            {
                deletedAudit = await _auditoriumService.DeleteAuditorium(id);
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

            if (deletedAudit == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.AUDITORIUM_NOT_FOUND,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }
            return Accepted("auditorium//" + deletedAudit.Id, deletedAudit);
        }
    }
}