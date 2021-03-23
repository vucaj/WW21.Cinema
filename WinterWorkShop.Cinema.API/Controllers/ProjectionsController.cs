using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class ProjectionsController : ControllerBase
    {
        private readonly IProjectionService _projectionService;

        public ProjectionsController(IProjectionService projectionService)
        {
            _projectionService = projectionService;
        }

        /// <summary>
        /// Gets all projections
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<ProjectionDomainModel>>> GetAsync()
        {
            IEnumerable<ProjectionDomainModel> projectionDomainModels;
           
            projectionDomainModels = await _projectionService.GetAllAsync();            

            if (projectionDomainModels == null)
            {
                projectionDomainModels = new List<ProjectionDomainModel>();
            }

            return Ok(projectionDomainModels);
        }

        [HttpGet]
        [Route("getAllFuture")]
        public async Task<ActionResult<IEnumerable<ProjectionDomainModel>>> GetAllFutureAsync()
        {
            IEnumerable<ProjectionDomainModel> projectionDomainModels;
           
            projectionDomainModels = await _projectionService.GetFutureProjections();            

            if (projectionDomainModels == null)
            {
                projectionDomainModels = new List<ProjectionDomainModel>();
            }

            return Ok(projectionDomainModels);
        }

        /// <summary>
        /// Adds a new projection
        /// </summary>
        /// <param name="projectionModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        //[Authorize(Roles = "admin")] 
        public async Task<ActionResult<ProjectionDomainModel>> PostAsync(CreateProjectionModel projectionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DateTime dateTime = DateTime.Parse(projectionModel.DateTime);

            if (dateTime < DateTime.Now)
            {
                ModelState.AddModelError(nameof(projectionModel.DateTime), Messages.PROJECTION_IN_PAST);
                return BadRequest(ModelState);
            }


            ProjectionDomainModel domainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = projectionModel.AuditoriumId,
                CinemaId = projectionModel.CinemaId,
                DateTime = dateTime,
                MovieId = projectionModel.MovieId,
                TicketPrice = projectionModel.TicketPrice
            };

            CreateProjectionResultModel createProjectionResultModel;

            try
            {
                createProjectionResultModel = await _projectionService.CreateProjection(domainModel);
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

            if (!createProjectionResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = createProjectionResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);                
            }

            return Created("projections//" + createProjectionResultModel.Projection.Id, createProjectionResultModel.Projection);
        }
        
        [HttpDelete]
        [Route("delete")]
        public async Task<ActionResult<ProjectionDomainModel>> DeleteProjectionAsync([FromBody] DeleteProjectionModel deleteProjectionModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProjectionDomainResultModel projection =
                await _projectionService.GetById(deleteProjectionModel.ProjectionId);

            if (!projection.IsSuccessful)
            {
                return BadRequest(new ProjectionDomainResultModel
                {
                  IsSuccessful  = false,
                  ErrorMessage = Messages.PROJECTION_NOT_FOUND,
                  Projection = null
                });
            }

            await _projectionService.DeleteProjection(projection.Projection);

            return Accepted("projections//" + projection.Projection.Id, projection.Projection);
        }

        [HttpGet]
        [Route("getAllFutureByMovieId")]
        public async Task<ActionResult<IEnumerable<ProjectionDomainModel>>> GetAllFutureProjectionsByMovieIdAsync([FromBody] MovieDomainModel domainModel)
        {
            IEnumerable<ProjectionDomainModel> projectionDomainModels;

            projectionDomainModels = await _projectionService.GetFutureProjectionsByMovieId(new MovieDomainModel
            {
                Id = domainModel.Id
            });

            if (projectionDomainModels == null)
            {
                projectionDomainModels = new List<ProjectionDomainModel>();
            }

            return Ok(projectionDomainModels);
        }
    }
}