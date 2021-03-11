using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class CinemasControllerTests
    {
        private Mock<ICinemaService> _cinemaService;

        [TestMethod]
        public void GetAsync_Return_NewList()
        {
            //Arrange
            IEnumerable<CinemaDomainModel> cinemaDomainModels = null;
            Task<IEnumerable<CinemaDomainModel>> responseTask = Task.FromResult(cinemaDomainModels);
            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);
            
            //Act
            var result = cinemasController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult) result).Value;
            var cinemaDomainModelResultList = (List<CinemaDomainModel>) resultList;
            
            //Assert
            Assert.IsNotNull(cinemaDomainModelResultList);
            Assert.AreEqual(expectedResultCount, cinemaDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAsync_Return_All_Cinemas()
        {
            //Arrange
            List<CinemaDomainModel> cinemaDomainModelsList = new List<CinemaDomainModel>();
            CinemaDomainModel cinemaDomainModel1 = new CinemaDomainModel
            {
                Id = Guid.NewGuid(),
                Name = "Ime1",
                AddressId = 1
            };
            CinemaDomainModel cinemaDomainModel2 = new CinemaDomainModel
            {
                Id = Guid.NewGuid(),
                Name = "Ime2",
                AddressId = 1
            };
            
            cinemaDomainModelsList.Add(cinemaDomainModel1);
            cinemaDomainModelsList.Add(cinemaDomainModel2);

            IEnumerable<CinemaDomainModel> cinemaDomainModels = cinemaDomainModelsList;
            Task<IEnumerable<CinemaDomainModel>> responseTask = Task.FromResult(cinemaDomainModels);

            int expectedResultCount = 2;
            int expectedStatusCode = 200;

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.GetAllAsync()).Returns(responseTask);

            CinemasController cinemasController = new CinemasController(_cinemaService.Object);
            
            //Act
            var result = cinemasController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult) result).Value;
            var cinemaDomainModelResultList = (List<CinemaDomainModel>) resultList;
            
            //Assert
            Assert.IsNotNull(cinemaDomainModelResultList);
            Assert.AreEqual(expectedResultCount, cinemaDomainModelResultList.Count);
            Assert.AreEqual(cinemaDomainModel1.Id, cinemaDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_CreateCinema_createCinemaResultModel_IsSuccesful_True_Cinema()
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateCinemaModel createCinemaModel = new CreateCinemaModel()
            {
                Name = "Ime1",
                AddressId = 1
            };

            CreateCinemaResultModel createCinemaResultModel = new CreateCinemaResultModel()
            {
                Cinema = new CinemaDomainModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Ime1",
                    AddressId = 1
                },
                IsSuccessful = true,
                ErrorMessage = null
            };

            Task<CreateCinemaResultModel> responseTask = Task.FromResult(createCinemaResultModel);

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.Create(It.IsAny<CinemaDomainModel>())).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.CreateCinemaAsync(createCinemaModel).ConfigureAwait(false).GetAwaiter()
                .GetResult().Result;
            var createdResult = ((CreatedResult)result).Value;
            var cinemaDomainModel = (CinemaDomainModel) createdResult;

            //Assert
            Assert.IsNotNull(cinemaDomainModel);
            Assert.AreEqual(createCinemaModel.AddressId, cinemaDomainModel.AddressId);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult) result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_CreateCinema_Throw_DbException_Cinema()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            CreateCinemaModel createCinemaModel = new CreateCinemaModel()
            {
                Name = "Ime1",
                AddressId = 1
            };

            CreateCinemaResultModel createCinemaResultModel = new CreateCinemaResultModel()
            {
                IsSuccessful = true,
                Cinema = new CinemaDomainModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "Ime1",
                    AddressId = 1
                }
            };

            Task<CreateCinemaResultModel> responseTask = Task.FromResult(createCinemaResultModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.Create(It.IsAny<CinemaDomainModel>())).Throws(dbUpdateException);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.CreateCinemaAsync(createCinemaModel).ConfigureAwait(false).GetAwaiter()
                .GetResult().Result;
            var resultResponse = (BadRequestObjectResult) result;
            var badObjectResult = ((BadRequestObjectResult) result).Value;
            var errorResult = (ErrorResponseModel) badObjectResult;
            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void PostAsync_Create_createCinemaResultModel_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Error occured while saving to database.";
            int expectedStatusCode = 400;

            CreateCinemaModel createCinemaModel = new CreateCinemaModel()
            {
                Name = "ime1",
                AddressId = 1
            };

            CreateCinemaResultModel createCinemaResultModel = new CreateCinemaResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.CINEMA_SAVE_ERROR,
                Cinema = new CinemaDomainModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "ime1",
                    AddressId = 1
                }
            };

            Task<CreateCinemaResultModel> responseTask = Task.FromResult(createCinemaResultModel);

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.Create(It.IsAny<CinemaDomainModel>())).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);
            
            //Act
            var result = cinemasController.CreateCinemaAsync(createCinemaModel).ConfigureAwait(false).GetAwaiter()
                .GetResult().Result;
            var resultResponse = (BadRequestObjectResult) result;
            var badObjectResult = ((BadRequestObjectResult) result).Value;
            var errorResult = (ErrorResponseModel) badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void PostAsync_With_Invalid_ModelState_Return_BadReques()
        {
            //Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            CreateCinemaModel createProjectionModel = new CreateCinemaModel()
            {
                Name = "Ime1",
                AddressId = 0
            };

            _cinemaService = new Mock<ICinemaService>();
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);
            cinemasController.ModelState.AddModelError("key", "Invalid Model State");

            //Act
            var result = cinemasController.CreateCinemaAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter()
                .GetResult().Result;
            var resultResponse = (BadRequestObjectResult) result;
            var createdResult = ((BadRequestObjectResult) result).Value;
            var errorResponse = ((SerializableError) createdResult).GetValueOrDefault("key");
            var message = (string[]) errorResponse;
            
            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        /*[TestMethod]
        public void PostAsync_DeleteCinema_Cinema()
        {
            //Arrange
            List<CinemaDomainModel> cinemaDomainModelsList = new List<CinemaDomainModel>();
            
            
            CinemaDomainModel cinemaDomainModel1 = new CinemaDomainModel
            {
                Id = Guid.NewGuid(),
                Name = "Ime1",
                AddressId = 1
            };
            
            cinemaDomainModelsList.Add(cinemaDomainModel1);

            
            DeleteCinemaModel deleteCinemaModel = new DeleteCinemaModel()
            {
                Id = cinemaDomainModel1.Id
            };

            DeleteCinemaResultModel deleteCinemaResultModel = new DeleteCinemaResultModel()
            {
                isSuccessful = true,
                ErrorMessage = null,
                Cinema = new CinemaDomainModel()
                {
                    Id = cinemaDomainModel1.Id,
                    Name = "Ime1",
                    AddressId = 1
                }
            };
            
            Task<DeleteCinemaResultModel> responseTask = Task.FromResult(deleteCinemaResultModel);
            
            int expectedStatusCode = 201;

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.Delete(It.IsAny<CinemaDomainModel>())).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            
            
            //Act
            var result = cinemasController.DeleteCinemaAsync(deleteCinemaModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var deletedResult = ((AcceptedResult) result).Value;
            var cinemaDomainModel = (CinemaDomainModel) deletedResult;
            
            //Assert
            Assert.IsNotNull(cinemaDomainModel);
            Assert.AreEqual(deleteCinemaModel.Id, cinemaDomainModel.Id);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult) result).StatusCode);
        }*/
        
        
    }
}