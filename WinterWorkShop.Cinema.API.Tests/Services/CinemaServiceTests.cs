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
    public class CinemaServiceTests
    {
        private Mock<ICinemasRepository> _mockCinemaRepository;
        private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
        private Mock<ISeatsRepository> _mockSeatRepository;
        private Mock<IAddressRepository> _mockAddressRepository;
        private CinemaService cinemaService;
        private Data.Cinema _cinema;
        private Address _address;
        private CinemaDomainModel _cinemaDomainModel;
        private AddressDomainModel _addressDomainModel;
        private Auditorium _auditorium;
        private Projection _projection;
        private Seat _seat;
        private Movie _movie;
        private Ticket _ticket;
        private User _user;

        [TestInitialize]
        public void TestInitialize()
        {
            List<Auditorium> auditoria = new List<Auditorium>();
            List<Ticket> tickets = new List<Ticket>();
            List<Seat> seats = new List<Seat>();
            List<Projection> projections = new List<Projection>();
            List<MovieParticipant> movieParticipants = new List<MovieParticipant>();
            List<Data.Cinema> cinemas = new List<Data.Cinema>();

            _address = new Address
            {
                Cinemas = cinemas,
                StreetName = "Nikole Pasica",
                Longitude = 20,
                Latitude = 20,
                Country = "Srbija",
                CityName = "Zrenjanin",
                Id = 1
            };

            _cinema = new Data.Cinema
            {
                Id = Guid.NewGuid(),
                AddressId = 1,
                Name = "Bioskop",
                Auditoria = auditoria,
                Address = _address
            };

            _movie = new Movie
            {
                Id = Guid.NewGuid(),
                Description = "Deskripcija",
                Distributer = "Distributer",
                Duration = 120,
                Projections = projections,
                Genre = Genre.ACTION,
                IsActive = false,
                MovieParticipants = movieParticipants,
                NumberOfOscars = 1,
                Rating = 5,
                Title = "Naslov",
                Year = 2021
            };

            /*            _projection = new Projection
                        {
                            Id = Guid.NewGuid(),
                            Cinema = _cinema,
                            CinemaId = _cinema.Id,
                            Auditorium = _auditorium,
                            AuditoriumId = _auditorium.Id,
                            DateTime = DateTime.Now,
                            Movie = _movie,
                            MovieId = _movie.Id,
                            TicketPrice = 350,
                            Tickets = tickets
                        };*/

            _auditorium = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = "Bioskop",
                Cinema = _cinema,
                CinemaId = _cinema.Id,
                Projections = projections,
                Seats = seats
            };

            _cinemaDomainModel = new CinemaDomainModel
            {
                Id = _cinema.Id,
                AddressId = _cinema.AddressId,
                Name = _cinema.Name,
                Country = _address.Country,
                Latitude = _address.Latitude,
                Longitude = _address.Longitude,
                StreetName = _address.StreetName,
                CityName = _address.CityName
            };

            _mockCinemaRepository = new Mock<ICinemasRepository>();
            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockSeatRepository = new Mock<ISeatsRepository>();
            _mockAddressRepository = new Mock<IAddressRepository>();

            cinemaService = new CinemaService(_mockCinemaRepository.Object, _mockAuditoriumRepository.Object, _mockSeatRepository.Object);
        }

        //GetAllCinemas tests
        [TestMethod]
        public void CinemaService_GetAllCinemasAsync_ReturnsListOfCinemas()
        {
            //Arrange
            List<Auditorium> auditoria = new List<Auditorium>();

            var expectedCount = 2;
            Data.Cinema cinema2 = new Data.Cinema
            {
                Id = Guid.NewGuid(),
                AddressId = 1,
                Name = "Bioskop",
                Auditoria = auditoria,
                Address = _address
            };
            CinemaDomainModel cinemaDomainModel2 = new CinemaDomainModel
            {
                Id = cinema2.Id,
                AddressId = cinema2.AddressId,
                Name = cinema2.Name,
                Country = _address.Country,
                Latitude = _address.Latitude,
                Longitude = _address.Longitude,
                StreetName = _address.StreetName,
                CityName = _address.CityName
            };
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Add(_cinema);
            cinemas.Add(cinema2);
            _mockCinemaRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(cinemas);

            //Act
            var resultAction = cinemaService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(_cinema.Name, result[0].Name);
            Assert.IsInstanceOfType(resultAction, typeof(IEnumerable<CinemaDomainModel>));
        }

        [TestMethod]
        public void CinemaService_GetAllCinemasAsync_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Clear();
            _mockCinemaRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(cinemas);

            //Act
            var resultAction = cinemaService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(resultAction, typeof(IEnumerable<CinemaDomainModel>));
        }

        //DeleteCinema tests
        [TestMethod]
        public void CinemaService_DeleteCinemaAsync_ReturnsDeletedCinema()
        {
            //Arrange
            List<Seat> seats1 = new List<Seat>();
            List<Seat> seats2 = new List<Seat>();
            List<Projection> projections = new List<Projection>();
            var seat1 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 90,
                Row = 10,
                AuditoriumId = _auditorium.Id,
                SeatType = SeatType.REGULAR,
                Auditorium = _auditorium
            };
            var seat2 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 91,
                Row = 10,
                AuditoriumId = _auditorium.Id,
                Auditorium = _auditorium,
                SeatType = SeatType.REGULAR
            };
            var seat3 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 92,
                Row = 10,
                AuditoriumId = _auditorium.Id,
                SeatType = SeatType.REGULAR,
                Auditorium = _auditorium
            };
            seats1.Add(seat1);
            seats1.Add(seat2);
            seats2.Add(seat3);

            Auditorium audit1 = new Auditorium
            {
                Id = Guid.NewGuid(),
                Cinema = _cinema,
                CinemaId = _cinema.Id,
                Name = "Sala1",
                Projections = projections,
                Seats = seats1
            };

            Auditorium audit2 = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = "Sala2",
                CinemaId = _cinema.Id,
                Cinema = _cinema,
                Projections = projections,
                Seats = seats2
            };

            List<Auditorium> auditsList = new List<Auditorium>();
            auditsList.Add(audit1);
            auditsList.Add(audit2);

            _mockCinemaRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_cinema);
            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(auditsList);

            //Act
            var resultAction = cinemaService.Delete(_cinema.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_cinema.Id, resultAction.Id);
            Assert.IsInstanceOfType(resultAction, typeof(CinemaDomainModel));
        }

        [TestMethod]
        public void CinemaService_DeleteCinemaAsync_ReturnsNullCinema()
        {
            //Arrange
            List<Projection> projections = new List<Projection>();
            List<Seat> seats = new List<Seat>();

            Auditorium audit1 = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = "Sala1",
                CinemaId = _cinema.Id,
                Cinema = _cinema,
                Projections = projections,
                Seats = seats
            };

            Auditorium audit2 = new Auditorium
            {
                Id = Guid.NewGuid(),
                Name = "Sala2",
                CinemaId = _cinema.Id,
                Cinema = _cinema,
                Projections = projections,
                Seats = seats
            };

            List<Auditorium> auditsList = new List<Auditorium>();
            auditsList.Add(audit1);
            auditsList.Add(audit2);


            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Data.Cinema);
            _mockAuditoriumRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(auditsList);
            _mockCinemaRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_cinema);

            //Act
            var resultAction = cinemaService.Delete(_cinema.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //CreateCinema tests
        [TestMethod]
        public void CinemaService_CreateCinemaAsync_ReturnsListOfCinemas()
        {
            //Arrange
            _addressDomainModel = new AddressDomainModel()
            {
                Id = _address.Id,
                CityName = _address.CityName,
                Country = _address.Country,
                Latitude = _address.Latitude,
                Longitude = _address.Longitude,
                StreetName = _address.StreetName
            };

            _mockCinemaRepository.Setup(x => x.Insert(It.IsAny<Data.Cinema>())).Returns(_cinema);
            _mockAddressRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_address);

            //Act
            var resultAction = cinemaService.Create(_cinemaDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_cinema.Name, resultAction.Name);
            Assert.IsInstanceOfType(resultAction, typeof(CinemaDomainModel));
        }

        [TestMethod]
        public void CinemaService_CreateCinemaAsync_ReturnsNull()
        {
            //Arrange
            _mockCinemaRepository.Setup(x => x.Insert(_cinema)).Returns(_cinema);

            //Act
            var resultAction = cinemaService.Create(_cinemaDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void CinemaService_CreateCinemaAsync_ReturnsListOfCinemasWithAuditoriumsSeats()
        {
            //Arrange
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            List<Projection> projections = new List<Projection>();
            List<Auditorium> auditoria = new List<Auditorium>();

            _addressDomainModel = new AddressDomainModel()
            {
                Id = _address.Id,
                CityName = _address.CityName,
                Country = _address.Country,
                Latitude = _address.Latitude,
                Longitude = _address.Longitude,
                StreetName = _address.StreetName
            };

            SeatDomainModel seat1 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                Number = 90,
                Row = 10,
                AuditoriumId = _auditorium.Id,
                SeatType = SeatType.REGULAR
            };

            SeatDomainModel seat2 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                Number = 90,
                Row = 11,
                AuditoriumId = _auditorium.Id,
                SeatType = SeatType.REGULAR
            };

            Seat s1 = new Seat
            {
                Id = seat1.Id,
                AuditoriumId = seat1.AuditoriumId,
                Number = seat1.Number,
                Row = seat1.Row,
                SeatType = seat1.SeatType,
                Auditorium = _auditorium
            };

            Seat s2 = new Seat
            {
                Id = seat2.Id,
                AuditoriumId = seat2.AuditoriumId,
                Number = seat2.Number,
                Row = seat2.Row,
                SeatType = seat2.SeatType,
                Auditorium = _auditorium
            };

            List<SeatDomainModel> seats = new List<SeatDomainModel>();
            seats.Add(seat1);
            seats.Add(seat2);

            List<Seat> ss = new List<Seat>();
            ss.Add(s1);
            ss.Add(s2);

            AuditoriumDomainModel auditoriumDomainModel = new AuditoriumDomainModel
            {
                Id = Guid.NewGuid(),
                Name = "Sala1",
                CinemaId = _cinema.Id
            };

            Auditorium auditorium = new Auditorium
            {
                Id = auditoriumDomainModel.Id,
                CinemaId = auditoriumDomainModel.CinemaId,
                Cinema = _cinema,
                Name = auditoriumDomainModel.Name,
                Projections = projections,
                Seats = ss
            };

            List<AuditoriumDomainModel> auditoriumDomainModelList = new List<AuditoriumDomainModel>();
            auditoriumDomainModelList.Add(auditoriumDomainModel);

            List<Auditorium> auditoriumList = new List<Auditorium>();
            auditoriumList.Add(auditorium);

            /*            _cinemaDomainModel.AuditoriumsList = auditoriumDomainModelList;
                        _cinema.Auditoriums = auditoriumList;*/

            int expectedAuditoriumCount = 1;

            _mockAddressRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_address);
            _mockCinemaRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(cinemas);
            _mockCinemaRepository.Setup(x => x.Insert(It.IsAny<Data.Cinema>())).Returns(_cinema);
            _mockCinemaRepository.Setup(x => x.Save());

            //Act
            var resultAction = cinemaService.Create(_cinemaDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedAuditoriumCount, auditoriumDomainModelList.Count);
            Assert.AreEqual(_cinemaDomainModel.Name, resultAction.Name);
            Assert.IsInstanceOfType(resultAction, typeof(CinemaDomainModel));
        }
    }
}

