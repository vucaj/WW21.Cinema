using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        /// <summary>
        /// Gets Movie by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-{id}")]
        public async Task<ActionResult<MovieDomainModel>> GetAsync(Guid id)
        {
            MovieDomainModel movie;

            movie = await _movieService.GetMovieByIdAsync(id);

            if (movie == null)
            {
                return NotFound(Messages.MOVIE_DOES_NOT_EXIST);
            }

            return Ok(movie);
        }

        /// <summary>
        /// Gets all current movies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("current")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetAsync()
        {
            IEnumerable<MovieDomainModel> movieDomainModels;

            movieDomainModels = _movieService.GetAllMovies(true);

            if (movieDomainModels == null)
            {
                movieDomainModels = new List<MovieDomainModel>();
            }

            return Ok(movieDomainModels);
        }

        [HttpGet]
        [Route("top")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetTop10Async()
        {
            IEnumerable<MovieDomainModel> movieDomainModels;

            movieDomainModels = await _movieService.GetTop10Async();

            if (movieDomainModels == null)
            {
                movieDomainModels = new List<MovieDomainModel>();
            }

            return Ok(movieDomainModels);
        }
        
        [HttpGet]
        [Route("top-{year}")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetTop10Async(int year)
        {
            if (!(year >= 1950 && year <= 2021))
            {
                return BadRequest();
            }
            
            IEnumerable<MovieDomainModel> movieDomainModels;

            movieDomainModels = await _movieService.GetTop10ByYearAsync(year);

            if (movieDomainModels == null)
            {
                movieDomainModels = new List<MovieDomainModel>();
            }

            return Ok(movieDomainModels);
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<MovieDomainModel>>> GetAllAsync()
        {
            IEnumerable<MovieDomainModel> movieDomainModels;

            movieDomainModels = await _movieService.GetAllMovies();

            if (movieDomainModels == null)
            {
                movieDomainModels = new List<MovieDomainModel>();
            }

            return Ok(movieDomainModels);
        }
        
        /// <summary>
        /// Adds a new movie
        /// </summary>
        /// <param name="movieModel"></param>
        /// <returns></returns>
        //[Authorize(Roles = "admin")]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Post([FromBody]MovieModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Title = movieModel.Title,
                Description = movieModel.Description,
                Genre = movieModel.Genre,
                Duration = movieModel.Duration,
                Rating = movieModel.Rating,
                Distributer = movieModel.Distributer,
                Year = movieModel.Year,
                IsActive = movieModel.IsActive,
                NumberOfOscars = movieModel.NumberOfOscars
            };

            MovieDomainModel createMovie;

            try
            {
                createMovie = await _movieService.AddMovie(domainModel);
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

            if (createMovie == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_CREATION_ERROR,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int)System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Created("movies//" + createMovie.Id, createMovie);
        }

        /// <summary>
        /// Updates a movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movieModel"></param>
        /// <returns></returns>
        //[Authorize(Roles = "admin")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody]MovieModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MovieDomainModel movieToUpdate = await _movieService.GetMovieByIdAsync(id);

            if (movieToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            
            movieToUpdate.Title = movieModel.Title;
            movieToUpdate.Description = movieModel.Description;
            movieToUpdate.Genre = movieModel.Genre;
            movieToUpdate.Duration = movieModel.Duration;
            movieToUpdate.Rating = movieModel.Rating;
            movieToUpdate.Distributer = movieModel.Distributer;
            movieToUpdate.Year = movieModel.Year;
            movieToUpdate.IsActive = movieModel.IsActive;
            movieToUpdate.NumberOfOscars = movieModel.NumberOfOscars;

            MovieDomainModel movieDomainModel;
            try
            {
                movieDomainModel = await _movieService.UpdateMovie(movieToUpdate);
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

            return Accepted("movies//" + movieDomainModel.Id, movieDomainModel);

        }

        /// <summary>
        /// Delete a movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[Authorize(Roles = "admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            MovieDomainModel deletedMovie;
            try
            {
                deletedMovie = await _movieService.DeleteMovie(id);
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

            if (deletedMovie == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };

                return StatusCode((int) System.Net.HttpStatusCode.InternalServerError, errorResponse);
            }

            return Accepted("movies//" + deletedMovie.Id, deletedMovie);
        }

        [HttpPut]
        [Route("activateDeactivate/{id}")]
        public async Task<ActionResult> ActivateDeactivateMovie(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MovieDomainModel movieToUpdate = await _movieService.GetMovieByIdAsync(id);

            if (movieToUpdate == null)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            movieToUpdate.IsActive = !movieToUpdate.IsActive;

            MovieDomainModel movieDomainModel;
            try
            {
                movieDomainModel = await _movieService.UpdateMovie(movieToUpdate);
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

            return Accepted("activate-deactivate//" + movieDomainModel.Id, movieDomainModel);

        }
    }
}
