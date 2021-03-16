using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class MovieParticipantControllerTests
    {
        private Mock<IMovieParticipantService> _movieParticipantService;
        private Mock<IMovieService> _movieService;

        [TestMethod]
        public void GetAllAsync_Return_All_MovieParticipants()
        {
            // Arrange
            List<MovieParticipantDomainModel> movieParticipantDomainModelsList = new List<MovieParticipantDomainModel>();
            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid()
            };
            movieParticipantDomainModelsList.Add(movieParticipantDomainModel);
            IEnumerable<MovieParticipantDomainModel> movieParticipantDomainModels = movieParticipantDomainModelsList;
            Task<IEnumerable<MovieParticipantDomainModel>> responseTask = Task.FromResult(movieParticipantDomainModels);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _movieParticipantService = new Mock<IMovieParticipantService>();
            _movieParticipantService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            MovieParticipantController movieParticipantController = new MovieParticipantController(_movieParticipantService.Object);

            // Act
            var result = movieParticipantController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var MovieParticipantDomainModelResultList = (List<MovieParticipantDomainModel>)resultList;

            // Assert
            Assert.IsNotNull(MovieParticipantDomainModelResultList);
            Assert.AreEqual(expectedResultCount, MovieParticipantDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAllMovieParticipants_Return_NewList()
        {
            // Arrange
            IEnumerable<MovieParticipantDomainModel> movieParticipantDomainModels = null;
            Task<IEnumerable<MovieParticipantDomainModel>> responseTask = Task.FromResult(movieParticipantDomainModels);
            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _movieParticipantService = new Mock<IMovieParticipantService>();
            _movieParticipantService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            MovieParticipantController MovieParticipantController = new MovieParticipantController(_movieParticipantService.Object);

            // Act
            var result = MovieParticipantController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieParticipantDomainModelResultList = (List<MovieParticipantDomainModel>)resultList;

            // Assert
            Assert.IsNotNull(movieParticipantDomainModelResultList);
            Assert.AreEqual(expectedResultCount, movieParticipantDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetMovieParticipantById_Return_MovieParticipant()
        {
            // Arrange
            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid()
            };

            GetMovieParticipantResultModel getMovieParticipantResultModel = new GetMovieParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                MovieParticipant = new MovieParticipantDomainModel
                {
                    Id = movieParticipantDomainModel.Id,
                    MovieId = movieParticipantDomainModel.MovieId,
                    ParticipantId = movieParticipantDomainModel.ParticipantId
                }
            };

            Task<GetMovieParticipantResultModel> responseTask = Task.FromResult(getMovieParticipantResultModel);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _movieParticipantService = new Mock<IMovieParticipantService>();
            _movieParticipantService.Setup(x => x.GetByMovieParticipantId(It.IsAny<MovieParticipantDomainModel>())).Returns(responseTask);
            MovieParticipantController movieParticipantController = new MovieParticipantController(_movieParticipantService.Object);

            // Act
            var result = movieParticipantController.GetMovieParticipantById(movieParticipantDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieParticipantDomainModelResultList = (GetMovieParticipantResultModel)resultList;

            // Assert
            Assert.IsNotNull(movieParticipantDomainModelResultList);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public async Task GetMovieParticipantById_WhenMovieParticipantIsNull_ReturnsNotFound_Tests()
        {
            // Arrange
            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                MovieId = Guid.NewGuid(),
                ParticipantId = Guid.NewGuid()
            };

            GetMovieParticipantResultModel getMovieParticipantResultModel = new GetMovieParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                MovieParticipant = new MovieParticipantDomainModel
                {
                    Id = movieParticipantDomainModel.Id,
                    MovieId = movieParticipantDomainModel.MovieId,
                    ParticipantId = movieParticipantDomainModel.ParticipantId
                }
            };

            Task<GetMovieParticipantResultModel> responseTask = Task.FromResult(getMovieParticipantResultModel);
            _movieParticipantService = new Mock<IMovieParticipantService>();
            _movieParticipantService.Setup(x => x.GetByMovieParticipantId(movieParticipantDomainModel)).Returns(responseTask);

            var movieParticipantController = new MovieParticipantController(_movieParticipantService.Object);

            // Act
            var test = await movieParticipantController.GetMovieParticipantById(movieParticipantDomainModel);

            // Assert
            Assert.IsInstanceOfType(test.Result, typeof(NotFoundObjectResult));
        }

        [TestMethod]
        public void GetMovieById_Return_MovieParticipant()
        {
            // Arrange
            List<GetMovieParticipantResultModel> movieDomainModelsList = new List<GetMovieParticipantResultModel>();
            MovieDomainModel movietDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid()
            };

            GetMovieParticipantResultModel getMovieParticipantResultModel = new GetMovieParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                MovieParticipant = new MovieParticipantDomainModel
                {
                    Id = movietDomainModel.Id
                }
            };

            movieDomainModelsList.Add(getMovieParticipantResultModel);
            IEnumerable<GetMovieParticipantResultModel> movieParticipantDomainModels = movieDomainModelsList;
            Task<IEnumerable<GetMovieParticipantResultModel>> responseTask = Task.FromResult(movieParticipantDomainModels);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _movieParticipantService = new Mock<IMovieParticipantService>();
            _movieParticipantService.Setup(x => x.GetAllByMovieIdAsync(It.IsAny<MovieDomainModel>())).Returns(responseTask);
            MovieParticipantController movieParticipantController = new MovieParticipantController(_movieParticipantService.Object);

            // Act
            var result = movieParticipantController.GetMovieById(movietDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieParticipantDomainModelResultList = (List<GetMovieParticipantResultModel>)resultList;

            // Assert
            Assert.IsNotNull(movieParticipantDomainModelResultList);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual(expectedResultCount, movieParticipantDomainModelResultList.Count);
        }

        [TestMethod]
        public void GetMovieById_WhenMovieParticipantIsNull_ReturnsNotFound_Tests()
        {
            //Arrange
            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = Guid.NewGuid()
            };

            GetMovieParticipantResultModel createMovieParticipantResultModel = new GetMovieParticipantResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIEPARTICIPANT_NOT_FOUND,
                MovieParticipant = movieParticipantDomainModel
            };

            Task<GetMovieParticipantResultModel> responseTask = Task.FromResult(createMovieParticipantResultModel);
            _movieParticipantService = new Mock<IMovieParticipantService>();
            _movieParticipantService.Setup(x => x.GetByMovieParticipantId(movieParticipantDomainModel)).Returns(responseTask);

            MovieParticipantController movieParticipantController = new MovieParticipantController(_movieParticipantService.Object);
            int expectedStatusCode = 404;

            //Act
            var result = movieParticipantController.GetMovieParticipantById(movieParticipantDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetParticipantById_Return_MovieParticipant()
        {
            // Arrange
            List<GetMovieParticipantResultModel> movieDomainModelsList = new List<GetMovieParticipantResultModel>();
            ParticipantDomainModel participantDomainModel = new ParticipantDomainModel
            {
                Id = Guid.NewGuid()
            };

            GetMovieParticipantResultModel getMovieParticipantResultModel = new GetMovieParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                MovieParticipant = new MovieParticipantDomainModel
                {
                    Id = participantDomainModel.Id
                }
            };

            movieDomainModelsList.Add(getMovieParticipantResultModel);
            IEnumerable<GetMovieParticipantResultModel> movieParticipantDomainModels = movieDomainModelsList;
            Task<IEnumerable<GetMovieParticipantResultModel>> responseTask = Task.FromResult(movieParticipantDomainModels);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _movieParticipantService = new Mock<IMovieParticipantService>();
            _movieParticipantService.Setup(x => x.GetAllByParticipantIdAsync(It.IsAny<ParticipantDomainModel>())).Returns(responseTask);
            MovieParticipantController movieParticipantController = new MovieParticipantController(_movieParticipantService.Object);

            // Act
            var result = movieParticipantController.GetParticipantById(participantDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieParticipantDomainModelResultList = (List<GetMovieParticipantResultModel>)resultList;

            // Assert
            Assert.IsNotNull(movieParticipantDomainModelResultList);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual(expectedResultCount, movieParticipantDomainModelResultList.Count);
        }

        [TestMethod]
        public void GetParticipantById_WhenMovieParticipantIsNull_ReturnsNotFound_Tests()
        {
            //Arrange
            MovieParticipantDomainModel movieParticipantDomainModel = new MovieParticipantDomainModel
            {
                Id = Guid.NewGuid()
            };

            GetMovieParticipantResultModel createMovieParticipantResultModel = new GetMovieParticipantResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIEPARTICIPANT_NOT_FOUND,
                MovieParticipant = movieParticipantDomainModel
            };

            Task<GetMovieParticipantResultModel> responseTask = Task.FromResult(createMovieParticipantResultModel);
            _movieParticipantService = new Mock<IMovieParticipantService>();
            _movieParticipantService.Setup(x => x.GetByMovieParticipantId(movieParticipantDomainModel)).Returns(responseTask);

            MovieParticipantController movieParticipantController = new MovieParticipantController(_movieParticipantService.Object);
            int expectedStatusCode = 404;

            //Act
            var result = movieParticipantController.GetMovieParticipantById(movieParticipantDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
        }
    }
}
