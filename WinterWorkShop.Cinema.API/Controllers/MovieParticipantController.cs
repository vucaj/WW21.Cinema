using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
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
        private readonly IMovieService _movieService;

        public MovieParticipantController(IMovieParticipantService movieParticipantService, IMovieService movieService)
        {
            _movieParticipantService = movieParticipantService;
            _movieService = movieService;
        }

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

            if (movieParticipantDomainModel == null)
            {
                return NotFound(Messages.MOVIEPARTICIPANT_NOT_FOUND);
            }

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

            if (movieParticipantByMovieId == null)
            {
                return NotFound(Messages.MOVIEPARTICIPANT_NOT_FOUND_BY_MOVIE_ID);
            }

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

            if (movieParticipantByParticipantId == null)
            {
                return NotFound(Messages.MOVIEPARTICIPANT_NOT_FOUND_BY_PARTICIPANT_ID);
            }

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

            var participantId = await _movieParticipantService.GetByParticipantIdAsync(createMovieParticipantModel.ParticipantId);
            var movieId = await _movieService.GetMovieByIdAsync(createMovieParticipantModel.MovieId);

            if (participantId == null || movieId == null)
            {
                return NotFound();
            }

            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                MovieId = createMovieParticipantModel.MovieId,
                ParticipantId = createMovieParticipantModel.ParticipantId
            };

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

            return Created("movie participant//" + createMovieParticipantResultModel.MovieParticipant.Id, createMovieParticipantResultModel.MovieParticipant);
        }

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<MovieParticipantDomainModel>> DeleteMovieParticipant([FromBody] DeleteMovieParticipantModel deleteMovieParticipantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GetMovieParticipantResultModel movieParticipantId = await _movieParticipantService.GetByMovieParticipantId(new MovieParticipantDomainModel
            {
                Id = deleteMovieParticipantModel.MovieParticipantId
            });

            if (!movieParticipantId.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel()
                {
                    ErrorMessage = movieParticipantId.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = deleteMovieParticipantModel.MovieParticipantId
            };

            DeleteMovieParticipantResultModel resultModel = await _movieParticipantService.Delete(movieParticipantDomainModel);

            if (!resultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = resultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            return Accepted("Deleted Movie Participant//" + resultModel.MovieParticipant.Id, resultModel.MovieParticipant);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<MovieParticipantDomainModel>> UpdateMovieParticipant([FromBody] UpdateMovieParticipantModel updateMovieParticipantModel)
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
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = updateMovieParticipantResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponseModel);
            }

            return Ok(updateMovieParticipantResultModel);
        }
    }
}
