using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Enums;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class AuditoriumServiceTests
    {
        private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
        private Mock<ICinemasRepository> _mockCinemaRepository;
        private Mock<ISeatsRepository> _mockSeatRepository;
        private Mock<IProjectionsRepository> _mockProjectionRepository;
        private Auditorium _auditorium;
        private Address _address;
        private Data.Cinema _cinema;
        private AuditoriumDomainModel _auditoriumDomainModel;
        private CinemaDomainModel _cinemaDomainModel;
        private AuditoriumService _auditoriumService;
        private AuditoriumService auditoriumService;

        [TestInitialize]
        public void TestInitialize()
        {
            List<Projection> projections = new List<Projection>();
            List<Seat> seats = new List<Seat>();
            List<Auditorium> auditoria = new List<Auditorium>();
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
                Name = "Bioskop",
                Auditoria = auditoria,
                AddressId = _address.Id
            };


            _auditorium = new Auditorium
            {
                Id = Guid.NewGuid(),
                CinemaId = _cinema.Id,
                Cinema = _cinema,
                Name = "Sala1",
                Projections = projections,
                Seats = seats
            };

            _auditoriumDomainModel = new AuditoriumDomainModel
            {
                Id = _auditorium.Id,
                CinemaId = _auditorium.CinemaId,
                Name = _auditorium.Name
            };

            _cinemaDomainModel = new CinemaDomainModel
            {
                Id = _cinema.Id,
                AddressId = _cinema.AddressId,
                Name = _cinema.Name,
                CityName = _address.CityName,
                Country = _address.Country,
                Latitude = _address.Latitude,
                Longitude = _address.Longitude,
                StreetName = _address.StreetName
            };


            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockCinemaRepository = new Mock<ICinemasRepository>();
            _mockProjectionRepository = new Mock<IProjectionsRepository>();
            _mockSeatRepository = new Mock<ISeatsRepository>();
            _auditoriumService = new AuditoriumService(_mockAuditoriumRepository.Object, _mockCinemaRepository.Object, _mockSeatRepository.Object, _mockProjectionRepository.Object);
        }

        //GetAllAuditoriums tests
        [TestMethod]
        public void AuditoriumService_GetAllAuditoriums_ReturnsListOfAuditoriums()
        {
            //Arrange
            List<Projection> projections = new List<Projection>();
            List<Seat> seats = new List<Seat>();
            var expectedCount = 2;

            Auditorium auditorium2 = new Auditorium
            {
                Id = Guid.NewGuid(),
                Cinema = _cinema,
                CinemaId = _cinema.Id,
                Name = "Sala14",
                Projections = projections,
                Seats = seats
            };

            List<Auditorium> auditoriums = new List<Auditorium>();
            auditoriums.Add(_auditorium);
            auditoriums.Add(auditorium2);

            List<AuditoriumDomainModel> auditoriumDomainModels = new List<AuditoriumDomainModel>();
            auditoriumDomainModels.Add(_auditoriumDomainModel);

            _mockAuditoriumRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(auditoriums);

            //Act
            var resultAction = _auditoriumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(result[0].Id, _auditoriumDomainModel.Id);
            Assert.IsInstanceOfType(result[0], typeof(AuditoriumDomainModel));
        }

        [TestMethod]
        public void AuditoriumService_GetAllAuditoriums_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            List<Auditorium> auditoriums = new List<Auditorium>();

            _mockAuditoriumRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(auditoriums);

            //Act
            var resultAction = _auditoriumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
        }

        //CreateAuditorium tests
        [TestMethod]
        public void AuditoriumService_CreateAuditorium_ReturnsCreatedAuditoriumWithSeats()
        {
            //Arrange
            var expectedSeatCount = 3;
            int numOfRows = 2;
            int numOfSeats = 4;

            Seat s1 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = _auditorium.Id,
                Number = 1,
                Row = 1,
                Auditorium = _auditorium,
                SeatType = SeatType.REGULAR
            };

            Seat s2 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = _auditorium.Id,
                Number = 2,
                Row = 1,
                Auditorium = _auditorium,
                SeatType = SeatType.REGULAR
            };

            Seat s3 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = _auditorium.Id,
                Number = 1,
                Row = 2,
                Auditorium = _auditorium,
                SeatType = SeatType.REGULAR
            };

            SeatDomainModel s4 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = _auditorium.Id,
                Number = 1,
                Row = 2,
                SeatType = SeatType.REGULAR
            };
            SeatDomainModel s5 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = _auditorium.Id,
                Number = 1,
                Row = 2,
                SeatType = SeatType.REGULAR
            };
            SeatDomainModel s6 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = _auditorium.Id,
                Number = 1,
                Row = 2,
                SeatType = SeatType.REGULAR
            };

            List<Auditorium> audits = new List<Auditorium>();

            List<Seat> seats = new List<Seat>();
            List<SeatDomainModel> seatsList = new List<SeatDomainModel>();
            seats.Add(s1);
            seats.Add(s2);
            seats.Add(s3);

            seatsList.Add(s4);
            seatsList.Add(s5);
            seatsList.Add(s6);

/*            _auditorium.Seats = seats;
            _auditoriumDomainModel.SeatsList = seatsList;*/
            AuditoriumDomainModel ad2 = new AuditoriumDomainModel
            {
                Id = Guid.NewGuid(),
                Name = "Sala13",
                CinemaId = _cinema.Id
            };

            CreateAuditoriumResultModel cr = new CreateAuditoriumResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Auditorium = _auditoriumDomainModel
            };

            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.Insert(It.IsAny<Auditorium>())).Returns(_auditorium);
            _mockAuditoriumRepository.Setup(x => x.GetByAuditName(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(audits);
            _mockAuditoriumRepository.Setup(x => x.Insert(It.IsAny<Auditorium>())).Returns(_auditorium);

            //Act
            var resultAction = _auditoriumService.CreateAuditorium(ad2, numOfRows, numOfSeats).ConfigureAwait(false).GetAwaiter().GetResult();

            //Arrange
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Auditorium.Id, cr.Auditorium.Id);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAuditoriumResultModel));
            Assert.AreEqual(expectedSeatCount, seatsList.Count);
            Assert.IsTrue(resultAction.IsSuccessful);
            Assert.IsNull(resultAction.ErrorMessage);
        }

        [TestMethod]
        public void AuditoriumService_CreateAuditorium_ReturnsErrorMessage_InvalidCinema()
        {
            //Arrange
            var numOfRows = 2;
            var numOfSeats = 3;

            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Data.Cinema);

            //Act
            var resultAction = _auditoriumService.CreateAuditorium(_auditoriumDomainModel, numOfRows, numOfSeats).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction.Auditorium);
            Assert.AreEqual(Messages.AUDITORIUM_INVALID_CINEMAID, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAuditoriumResultModel));
        }

/*        [TestMethod]
        public void AuditoriumService_CreateAuditorium_ReturnsErrorMessage_AuditoriumSameName()
        {
            //Arrange
            var numOfRows = 2;
            var numOfSeats = 3;

            List<Auditorium> audits = new List<Auditorium>();
            audits.Add(_auditorium);

            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetByAuditName(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(audits);

            //Act
            var resultAction = _auditoriumService.CreateAuditorium(_auditoriumDomainModel, numOfRows, numOfSeats).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction.Auditorium);
            Assert.AreEqual(Messages.AUDITORIUM_SAME_NAME, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAuditoriumResultModel));
        }*/

        [TestMethod]
        public void AuditoriumService_CreateAuditorium_ReturnsErrorMessage_AuditoriumCreationError()
        {
            //Arrange
            var numOfRows = 2;
            var numOfSeats = 3;

            List<Auditorium> audits = new List<Auditorium>();
            audits.Add(_auditorium);

            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetByAuditName(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(audits);

            //Act
            var resultAction = _auditoriumService.CreateAuditorium(_auditoriumDomainModel, numOfRows, numOfSeats).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction.Auditorium);
            Assert.AreEqual(Messages.AUDITORIUM_CREATION_ERROR, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAuditoriumResultModel));
        }

        //GetAuditoriumByCinemaId tests
/*        [TestMethod]
        public void AuditoriumService_GetAuditoriumsByCinemaId_ReturnsListOfAuditoriums()
        {
            //Arrange
            List<Seat> seats = new List<Seat>();
            List<Projection> projections = new List<Projection>();
            List<Auditorium> auditoria = new List<Auditorium>();
            var expectedCount = 2;
            Auditorium audit2 = new Auditorium
            {
                Id = Guid.NewGuid(),
                Cinema = _cinema,
                CinemaId = _cinema.Id,
                Name = "Sala14",
                Projections = projections,
                Seats = seats
            };

            Task<IEnumerable<Auditorium>> audits = new Task<IEnumerable<Auditorium>>().Result.ToList();
            audits.Add(_auditorium);
            audits.Add(audit2);

            _mockAuditoriumRepository.Setup(x => x.GetByCinemaId(It.IsAny<Guid>())).Returns(audits);

            //Act
            var resultAction = _auditoriumService.GetAllByCinemaIdAsync(_cinemaDomainModel);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(result[0].Id, _auditorium.Id);
            Assert.AreEqual(result[1].Name, audit2.AuditName);
            Assert.IsInstanceOfType(result[0], typeof(AuditoriumDomainModel));
        }*/

/*        [TestMethod]
        public void AuditoriumService_GetAuditoriumsByCinemaId_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            IEnumerable<Auditorium> auditoriumDomainModels = null;
            Task<IEnumerable<Auditorium>> responseTask = Task.FromResult(auditoriumDomainModels);

            _mockAuditoriumRepository.Setup(x => x.GetByCinemaId(It.IsAny<Guid>())).Returns(responseTask);

            //Act
            var resultAction = _auditoriumService.GetAllByCinemaIdAsync(_cinemaDomainModel);
            var result = resultAction.Result.ToList();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
        }*/
    }
}
