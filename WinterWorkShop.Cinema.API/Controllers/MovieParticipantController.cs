using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MovieParticipantController : ControllerBase
    {
        private readonly IMovieParticipantService _movieParticipantService;

        public MovieParticipantController(IMovieParticipantService movieParticipantService)
        {
            _movieParticipantService = movieParticipantService;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<MovieParticipantDomainModel>>> GetAllAsync()
        {
            IEnumerable<MovieParticipantDomainModel> movieParticipantDomainModels;

            movieParticipantDomainModels = await _movieParticipantService.GetAllAsync();

            if (movieParticipantDomainModels == null)
            {
                movieParticipantDomainModels = new List<MovieParticipantDomainModel>();
            }

            return Ok(movieParticipantDomainModels);
        }

        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<MovieParticipantDomainModel>> GetMovieParticipantById([FromBody] MovieParticipantDomainModel domainModel)
        {
            var movieParticipantDomainModel = await _movieParticipantService.GetByMovieParticipantId(new MovieParticipantDomainModel
            {
                Id = domainModel.Id
            });

            return Ok(movieParticipantDomainModel);
        }

        [HttpGet]
        [Route("getByMovieId")]
        public async Task<ActionResult<MovieParticipantDomainModel>> GetMovieById([FromBody] MovieDomainModel domainModel)
        {
            var movieParticipantByMovieId = await _movieParticipantService.GetAllByMovieIdAsync(new MovieDomainModel
            {
                Id = domainModel.Id
            });

            return Ok(movieParticipantByMovieId);
        }

        [HttpGet]
        [Route("getByParticipantId")]
        public async Task<ActionResult<MovieParticipantDomainModel>> GetParticipantById([FromBody] ParticipantDomainModel domainModel)
        {
            var movieParticipantByParticipantId = await _movieParticipantService.GetAllByParticipantIdAsync(new ParticipantDomainModel
            {
                Id = domainModel.Id
            });

            return Ok(movieParticipantByParticipantId);
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<MovieParticipantDomainModel>> CreateMovieParticipantAsync([FromBody] CreateMovieParticipantModel createMovieParticipantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                MovieId = createMovieParticipantModel.MovieId,
                ParticipantId = createMovieParticipantModel.ParticipantId
            };

            // todo: dodati i proveru za movie, isto kao sto je uradjen za participant
            var participantId = await _movieParticipantService.GetByParticipantIdAsync(movieParticipantDomainModel.ParticipantId);

            if(participantId == null)
            {
                return NotFound();
            }

            CreateMovieParticipantResultModel createMovieParticipantResultModel;

            try
            {
                createMovieParticipantResultModel = await _movieParticipantService.Create(movieParticipantDomainModel);
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

            if (!createMovieParticipantResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = createMovieParticipantResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Created("movie participant//" + createMovieParticipantResultModel.MovieParticipant.Id, createMovieParticipantResultModel);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult> DeleteMovieParticipant([FromBody] DeleteMovieParticipantModel deleteMovieParticipantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieParticipantId = await _movieParticipantService.GetByMovieParticipantId(new MovieParticipantDomainModel
            {
                Id = deleteMovieParticipantModel.MovieParticipantId
            });

            if(movieParticipantId == null)
            {
                return NotFound();
            }

            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = deleteMovieParticipantModel.MovieParticipantId
            };

            DeleteMovieParticipantResultModel deleteMovieParticipantResultModel;

            try
            {
                deleteMovieParticipantResultModel = await _movieParticipantService.Delete(movieParticipantDomainModel);
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

            if (!deleteMovieParticipantResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel()
                {
                    ErrorMessage = deleteMovieParticipantResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            return Ok("Deleted movie participant: " + movieParticipantDomainModel.Id);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult> UpdateMovieParticipant([FromBody] UpdateMovieParticipantModel updateMovieParticipantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieParticipant = await _movieParticipantService.GetByMovieParticipantId(new MovieParticipantDomainModel
            {
                Id = updateMovieParticipantModel.Id
            });

            movieParticipant.MovieParticipant.MovieId = updateMovieParticipantModel.MovieId;
            movieParticipant.MovieParticipant.ParticipantId = updateMovieParticipantModel.ParticipantId;

            UpdateMovieParticipantResultModel updateMovieParticipantResultModel = await _movieParticipantService.Update(new MovieParticipantDomainModel
            {
                Id = movieParticipant.MovieParticipant.Id,
                MovieId = movieParticipant.MovieParticipant.MovieId,
                ParticipantId = movieParticipant.MovieParticipant.ParticipantId
            });

            if (!updateMovieParticipantResultModel.IsSuccessful)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
