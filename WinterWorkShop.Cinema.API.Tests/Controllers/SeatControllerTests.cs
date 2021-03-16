using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.Data.Enums;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class SeatControllerTests
    {
        private Mock<ISeatService> _seatService;
        
        [TestMethod]
        public void GetAsync_Return_All_Seat()
        {
            //Arrange
            List<SeatDomainModel> seatDomainModelsList = new List<SeatDomainModel>();
            SeatDomainModel seatDomainModel = new SeatDomainModel()
            {
                Id = Guid.NewGuid(),
                AuditoriumId = Guid.NewGuid(),
                Number = 1,
                Row = 1,
                SeatType = SeatType.VIP
            };

            seatDomainModelsList.Add(seatDomainModel);
            IEnumerable<SeatDomainModel> seatDomainModels = seatDomainModelsList;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seatDomainModels);

            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _seatService = new Mock<ISeatService>();
            _seatService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object);

            //Act
            var result = seatsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var seatsDomainModelResultList = (List<SeatDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(seatsDomainModelResultList);
            Assert.AreEqual(expectedResultCount, seatsDomainModelResultList.Count);
            Assert.AreEqual(seatDomainModel.Id, seatsDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void GetAsync_Return_NewList()
        {
            //Arrange
            IEnumerable<SeatDomainModel> seatDomainModels = null;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seatDomainModels);

            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _seatService = new Mock<ISeatService>();
            _seatService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object);

            //Act
            var result = seatsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var seatDomainModelResultList = (List<SeatDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(seatDomainModelResultList);
            Assert.AreEqual(expectedResultCount, seatDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void GetSeatById_Return_Seat()
        {
            //Arrange
            SeatDomainModel seatDomainModel = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = Guid.NewGuid(),
                Number = 1,
                Row = 1,
                SeatType = SeatType.VIP
            };
            GetSeatResultModel getSeatResultModel = new GetSeatResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Seat = new SeatDomainModel
                {
                    Id = seatDomainModel.Id,
                    AuditoriumId = seatDomainModel.AuditoriumId,
                    Number = seatDomainModel.Number,
                    Row = seatDomainModel.Row,
                    SeatType = seatDomainModel.SeatType
                }
            };

            Task<GetSeatResultModel> responseTask = Task.FromResult(getSeatResultModel);
            int expectedStatusCode = 200;

            _seatService = new Mock<ISeatService>();
            _seatService.Setup(x => x.GetByIdAsync(It.IsAny<SeatDomainModel>())).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object);

            //Act
            var result = seatsController.GetByIdAsync(seatDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var seatsDomainModelResultList = (GetSeatResultModel)resultList;

            //Assert
            Assert.IsNotNull(seatsDomainModelResultList);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void GetAddressById_WhenSeatIsNull_ReturnsNotFound_Tests()
        {
            //Arrange
            SeatDomainModel seatDomainModel = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = Guid.NewGuid(),
                Number = 1,
                Row = 1,
                SeatType = SeatType.VIP
            };

            GetSeatResultModel createSeatResultModel = new GetSeatResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.SEAT_NOT_FOUND,
                Seat = seatDomainModel
            };

            Task<GetSeatResultModel> responseTask = Task.FromResult(createSeatResultModel);
            _seatService = new Mock<ISeatService>();
            _seatService.Setup(x => x.GetByIdAsync(seatDomainModel)).Returns(responseTask);

            SeatsController seatsController = new SeatsController(_seatService.Object);
            int expectedStatusCode = 404;

            //Act
            var result = seatsController.GetByIdAsync(seatDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundResult)result).StatusCode);
        }
    }
}
