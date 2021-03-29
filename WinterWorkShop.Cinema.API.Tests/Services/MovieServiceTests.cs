using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Enums;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class MovieServiceTests
    {
        private Mock<IMoviesRepository> _mockMovieRepository;
        private Mock<IProjectionsRepository> _mockProjectionRepository;
        private Movie _movie;
        private MovieDomainModel _movieDomainModel;

        [TestInitialize]
        public void TestInitilazie()
        {
            List<Projection> projections = new List<Projection>();
            List<MovieParticipant> movieParticipants = new List<MovieParticipant>();

            _movie = new Movie
            {
                Id = Guid.NewGuid(),
                Description = "Jako dobar film",
                Distributer = "SERBIA",
                Duration = 120,
                Genre = Genre.ACTION,
                IsActive = false,
                NumberOfOscars = 1,
                Rating = 5,
                Title = "Film",
                Year = 2021,
                MovieParticipants = movieParticipants,
                Projections = projections
            };

            _movieDomainModel = new MovieDomainModel
            { 
                Id = _movie.Id,
                Year = _movie.Year,
                Description = _movie.Description,
                Distributer = _movie.Distributer,
                Duration = _movie.Duration,
                Genre = _movie.Genre,
                IsActive = _movie.IsActive,
                NumberOfOscars = _movie.NumberOfOscars,
                Rating = _movie.Rating,
                Title = _movie.Title
            };

            List<Movie> movieModelsList = new List<Movie>();

            movieModelsList.Add(_movie);
            IEnumerable<Movie> movies = movieModelsList;
            Task<IEnumerable<Movie>> responseTask = Task.FromResult(movies);

            _mockMovieRepository = new Mock<IMoviesRepository>();
            _mockProjectionRepository = new Mock<IProjectionsRepository>();
            _mockMovieRepository.Setup(x => x.GetCurrentMovies()).Returns(movieModelsList);
            _mockMovieRepository.Setup(x => x.Insert(It.IsAny<Movie>())).Returns(_movie);
            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);
        }

        // GetAllMovies tests
        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies()
        {
            // Arrange
            int expectedCount = 1;
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            // Act
            var resultAction = movieService.GetAllMovies(true);
            var result = resultAction.ToList();

            // Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(_movie.Title, result[0].Title);
            Assert.AreEqual(_movie.Rating, result[0].Rating);
            Assert.AreEqual(_movie.Year, result[0].Year);
            Assert.IsInstanceOfType(result[0], typeof(MovieDomainModel));
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsNull()
        {
            //Arrange
            IEnumerable<Movie> movies = null;
            Task<IEnumerable<Movie>> responseTask = Task.FromResult(movies);
            _mockMovieRepository.Setup(x => x.GetCurrentMovies()).Returns(movies);
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true);

            //Assert
            Assert.IsNull(resultAction);
        }

        //GetMovieById tests
        [TestMethod]
        public void MovieService_GetMovieByIdAsync_ReturnsMovie()
        {
            //Arrange
            MovieService movieService = new MovieService(_mockMovieRepository.Object);
            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);

            //Act
            var resultAction = movieService.GetMovieByIdAsync(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_movie.Title, resultAction.Title);
            Assert.IsInstanceOfType(resultAction, typeof(MovieDomainModel));
        }

        [TestMethod]
        public void MovieService_GetMovieByIdAsync_ReturnsNull()
        {
            //Arrange 
            MovieService movieService = new MovieService(_mockMovieRepository.Object);
            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Movie);

            //Act
            var resultAction = movieService.GetMovieByIdAsync(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //CreateMovie tests
        [TestMethod]
        public void MovieService_AddMovie_ReturnsCreatedMovie()
        {
            //Arrange
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.AddMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_movieDomainModel.Title, resultAction.Title);
            Assert.AreEqual(_movie.Id, resultAction.Id);
            Assert.IsInstanceOfType(resultAction, typeof(MovieDomainModel));
        }

        [TestMethod]
        public void MovieService_UpdateMovie_ReturnsUpdatedMovie()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.Update(It.IsAny<Movie>())).Returns(_movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.UpdateMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
        }

        //DeleteMovie tests
        [TestMethod]
        public void MovieService_DeleteMovie_ReturnsDeletedMovie()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.DeleteMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_movieDomainModel.Title, resultAction.Title);
            Assert.IsInstanceOfType(resultAction, typeof(MovieDomainModel));
        }

        [TestMethod]
        public void MovieService_DeleteMovie_ReturnsNull()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(null as Movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.DeleteMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //GetTop10Movies tests
        [TestMethod]
        public void MovieService_GetTop10Movies_ReturnsMovieList()
        {
            //Arrange
            var expectedCount = 2;
            Movie movie2 = new Movie
            {
                Id = Guid.NewGuid(),
                IsActive = false,
                NumberOfOscars = 1,
                Rating = 3,
                Title = "Film",
                Year = 1999,
                Description = "Opis",
                Distributer = "SRBIJA",
                Duration = 120,
                Genre = Genre.ACTION
            };
            List<Movie> movieList = new List<Movie>();
            movieList.Add(_movie);
            movieList.Add(movie2);
            _mockMovieRepository.Setup(x => x.GetTop10()).Returns(movieList);
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.GetTop10Async().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result, typeof(List<MovieDomainModel>));
            Assert.IsInstanceOfType(resultAction, typeof(IEnumerable<MovieDomainModel>));
        }

        [TestMethod]
        public void MovieService_GetTop10Movies_ReturnsNull()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.GetTop10()).Returns(null as List<Movie>);
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.GetTop10Async().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //GetTop10ByYearAsync tests
        [TestMethod]
        public void MovieService_GetTop10ByYearAsync_ReturnsMovieList()
        {
            //Arrange
            var expectedCount = 2;
            Movie movie2 = new Movie
            {
                Id = Guid.NewGuid(),
                IsActive = false,
                NumberOfOscars = 1,
                Rating = 3,
                Title = "Film",
                Year = 1999,
                Description = "Opis",
                Distributer = "SRBIJA",
                Duration = 120,
                Genre = Genre.ACTION
            };
            List<Movie> movieList = new List<Movie>();
            movieList.Add(_movie);
            movieList.Add(movie2);
            _mockMovieRepository.Setup(x => x.GetTop10ByYear(It.IsAny<int>())).Returns(movieList);
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.GetTop10ByYearAsync(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result, typeof(List<MovieDomainModel>));
            Assert.IsInstanceOfType(resultAction, typeof(IEnumerable<MovieDomainModel>));
        }

        [TestMethod]
        public void MovieService_GetTop10ByYearAsync_ReturnsNull()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.GetTop10ByYear(It.IsAny<int>())).Returns(null as List<Movie>);
            MovieService movieService = new MovieService(_mockMovieRepository.Object);

            //Act
            var resultAction = movieService.GetTop10ByYearAsync(It.IsAny<int>()).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }
    }
}
