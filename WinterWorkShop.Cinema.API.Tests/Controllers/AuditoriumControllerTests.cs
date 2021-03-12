using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class AuditoriumControllerTests
    {
        private Mock<IAuditoriumService> _auditoriumService;

        [TestMethod]
        public void GetAsync_Return_All_Auditoria()
        {
            //Arrange
            List<AuditoriumDomainModel> auditoriumDomainModelsList = new List<AuditoriumDomainModel>();
            AuditoriumDomainModel auditoriumDomainModel = new AuditoriumDomainModel()
            {
                Id = Guid.NewGuid(),
                Name = "Sala1",
                CinemaId = Guid.NewGuid()
            };
            
            auditoriumDomainModelsList.Add(auditoriumDomainModel);
            IEnumerable<AuditoriumDomainModel> auditoriumDomainModels = auditoriumDomainModelsList;
            Task<IEnumerable<AuditoriumDomainModel>> responseTask = Task.FromResult(auditoriumDomainModels);

            int expectedResultCount = 1;
            int exepectedStatusCode = 200;

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            //Act
            var result = auditoriumsController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult) result).Value;
            var auditoriumDomainModelResultList = (List<AuditoriumDomainModel>) resultList;

            //Assert
            Assert.IsNotNull(auditoriumDomainModelResultList);
            Assert.AreEqual(expectedResultCount, auditoriumDomainModelResultList.Count);
            Assert.AreEqual(auditoriumDomainModel.Id, auditoriumDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(exepectedStatusCode, ((OkObjectResult) result).StatusCode);
        }

        [TestMethod]
        public void GetAsync_Return_NewList()
        {
            //Arrange
            IEnumerable<AuditoriumDomainModel> auditoriumDomainModels = null;
            Task<IEnumerable<AuditoriumDomainModel>> responseTask = Task.FromResult(auditoriumDomainModels);

            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            //Act
            var result = auditoriumsController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult) result).Value;
            var auditoriumDomainModelResultList = (List<AuditoriumDomainModel>) resultList;

            //Asset
            Assert.IsNotNull(auditoriumDomainModelResultList);
            Assert.AreEqual(expectedResultCount, auditoriumDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult) result).StatusCode);
            
        }
        
        [TestMethod]
        public void PostAsync_Create_createAuditoriumResultModel_IsSuccessful_True_Auditorium()
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateAuditoriumModel createAuditoriumModel = new CreateAuditoriumModel()
            {
                CinemaId = Guid.NewGuid(),
                Name = "Sala1",
                NumberOfSeats = 5,
                SeatRows = 5
            };

            CreateAuditoriumResultModel createAuditoriumResultModel = new CreateAuditoriumResultModel()
            {
                IsSuccessful = true,
                Auditorium = new AuditoriumDomainModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Sala1",
                    CinemaId = Guid.NewGuid()
                },
                ErrorMessage = null
            };

            Task<CreateAuditoriumResultModel> responseTask = Task.FromResult(createAuditoriumResultModel);

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.CreateAuditorium(It.IsAny<AuditoriumDomainModel>(), 5, 5)).Returns(responseTask);
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            //Act
            var result = auditoriumsController.CreateAuditoriumAsync(createAuditoriumModel).ConfigureAwait(false)
                .GetAwaiter().GetResult().Result;
            var createdResult = ((CreatedResult) result).Value;
            var auditoriumDomainModel = (AuditoriumDomainModel) createdResult;

            //Assert
            Assert.IsNotNull(auditoriumDomainModel);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult) result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_Create_Throw_DbException_Auditorium()
        {
            // Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            CreateAuditoriumModel createAuditoriumModel = new CreateAuditoriumModel()
            {
                CinemaId = Guid.NewGuid(),
                Name = "Sala1",
                NumberOfSeats = 5,
                SeatRows = 5
            };

            CreateAuditoriumResultModel createAuditoriumResultModel = new CreateAuditoriumResultModel()
            {
                Auditorium = new AuditoriumDomainModel()
                {
                    Id = Guid.NewGuid(),
                    CinemaId = createAuditoriumModel.CinemaId,
                    Name = "sala1"
                },
                IsSuccessful = true
            };

            Task<CreateAuditoriumResultModel> responseTask = Task.FromResult(createAuditoriumResultModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error", exception);

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.CreateAuditorium(It.IsAny<AuditoriumDomainModel>(), 5, 5))
                .Throws(dbUpdateException);
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            // Act
            var result = auditoriumsController.CreateAuditoriumAsync(createAuditoriumModel).ConfigureAwait(false)
                .GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult) result;
            var badObjectResult = ((BadRequestObjectResult) result).Value;
            var errorResult = (ErrorResponseModel) badObjectResult;

            // Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void PostAsync_Create_createAuditoriumResultModel_IsSuccessful_False_Return_BadRequest()
        {
            // Arrange
            string expectedMessage = "Error occured while creating new auditorium, please try again.";
            int expectedStatusCode = 400;

            CreateAuditoriumModel createAuditoriumModel = new CreateAuditoriumModel()
            {
                CinemaId = Guid.NewGuid(),
                Name = "Sala1",
                NumberOfSeats = 5,
                SeatRows = 5
            };

            CreateAuditoriumResultModel createAuditoriumResultModel = new CreateAuditoriumResultModel()
            {
                Auditorium = new AuditoriumDomainModel()
                {
                    CinemaId = createAuditoriumModel.CinemaId,
                    Id = Guid.NewGuid(),
                    Name = "Sala1"
                },
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_CREATION_ERROR
            };

            Task<CreateAuditoriumResultModel> responseTask = Task.FromResult(createAuditoriumResultModel);

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.CreateAuditorium(It.IsAny<AuditoriumDomainModel>(), 5, 5))
                .Returns(responseTask);
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            // Act
            var result = auditoriumsController.CreateAuditoriumAsync(createAuditoriumModel).ConfigureAwait(false)
                .GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult) result;
            var badObjectResult = ((BadRequestObjectResult) result).Value;
            var errorResult = (ErrorResponseModel) badObjectResult;

            // Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
            
        }

        [TestMethod]
        public void PostAsync_With_Invalid_ModelState_Return_BadRequest()
        {
            // Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            CreateAuditoriumModel createAuditoriumModel = new CreateAuditoriumModel()
            {
                CinemaId = Guid.NewGuid(),
                Name = "Sala1",
                NumberOfSeats = 5,
                SeatRows = 5
            };

            _auditoriumService = new Mock<IAuditoriumService>();
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);
            auditoriumsController.ModelState.AddModelError("key", "Invalid Model State");
            
            // Act
            var result = auditoriumsController.CreateAuditoriumAsync(createAuditoriumModel).ConfigureAwait(false)
                .GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult) result;
            var createdResult = ((BadRequestObjectResult) result).Value;
            var errorResponse = ((SerializableError) createdResult).GetValueOrDefault("key");
            var message = (string[]) errorResponse;
            
            // Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
            
        }
    }
}