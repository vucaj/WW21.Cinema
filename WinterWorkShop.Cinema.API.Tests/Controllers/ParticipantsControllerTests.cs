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
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Enums;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class ParticipantsControllerTests
    {
        private Mock<IParticipantService> _participantService;

        [TestMethod]
        public void GetAllAsync_Return_All_Participants()
        {
            // Arrange
            List<ParticipantDomainModel> participantDomainModelsList = new List<ParticipantDomainModel>();
            ParticipantDomainModel participantDomainModel = new ParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };
            participantDomainModelsList.Add(participantDomainModel);
            IEnumerable<ParticipantDomainModel> participantDomainModels = participantDomainModelsList;
            Task<IEnumerable<ParticipantDomainModel>> responseTask = Task.FromResult(participantDomainModels);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.GetAllParticipantsAsync()).Returns(responseTask);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var participantDomainModelResultList = (List<ParticipantDomainModel>)resultList;

            // Assert
            Assert.IsNotNull(participantDomainModelResultList);
            Assert.AreEqual(expectedResultCount, participantDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

/*        [TestMethod]
        public void GetParticipantById_Return_ParticipantById()
        {
            // Arrange
            ParticipantDomainModel participantDomainModel = new ParticipantDomainModel()
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };
            Task<ParticipantDomainModel> responseTask = Task.FromResult(participantDomainModel);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.GetParticipantByIdAsync(participantDomainModel));
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.GetParticipantById(participantDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var oneResult = ((OkObjectResult)result).Value;
            var participantDomainModelResult = (ParticipantDomainModel)oneResult;

            // Assert
            Assert.IsNotNull(participantDomainModelResult);
            Assert.AreEqual(expectedResultCount, participantDomainModelResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
*/
        [TestMethod]
        public void GetAllParticipants_Return_NewList()
        {
            // Arrange
            IEnumerable<ParticipantDomainModel> participantDomainModels = null;
            Task<IEnumerable<ParticipantDomainModel>> responseTask = Task.FromResult(participantDomainModels);
            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.GetAllParticipantsAsync()).Returns(responseTask);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var participantDomainModelResultList = (List<ParticipantDomainModel>)resultList;

            // Assert
            Assert.IsNotNull(participantDomainModelResultList);
            Assert.AreEqual(expectedResultCount, participantDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        /*[TestMethod]
        public async void GetParticipantById_Return_ParticipantById()
        {
            // Arrange
            var expectedParticipant = new ParticipantDomainModel();
            var expectedId = Guid.NewGuid();
            var participant = new ParticipantDomainModel()
            { 
                Id = expectedId
            };

            // proveriti zasto ne radi!!!!!!!!
            _participantService.Setup(srvc => srvc.GetParticipantByIdAsync(participant)).ReturnsAsync(expectedParticipant);

            var participantController = new ParticipantController(_participantService.Object);

            // Act
            var test = await participantController.GetParticipantById(participant);
            _participantService
                .Verify(srvc => srvc.GetParticipantByIdAsync(participant), Times.Exactly(1));
            // Assert
            Assert.IsInstanceOfType(test, typeof(ActionResult<ParticipantDomainModel>));
            Assert.IsInstanceOfType(test.Result, typeof(OkObjectResult));

            var okResult = ((OkObjectResult)test.Result).Value;
            var participant1 = (ParticipantDomainModel)okResult;

            Assert.AreSame(participant1, expectedParticipant);
        }*/

        [TestMethod]
        public void CreateParticipantAsync_Create_createParticipantResultModel_IsSuccessful_True_Participant()
        {
            // Arrange
            int expectedStatusCode = 201;

            CreateParticipantModel createPartcipantModel = new CreateParticipantModel()
            {
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.DIRECTOR
            };
            CreateParticipantResultModel createParticipantResultModel = new CreateParticipantResultModel
            {
                Participant = new ParticipantDomainModel
                {
                    Id = Guid.NewGuid(),
                    FirstName = createPartcipantModel.FirstName,
                    LastName = createPartcipantModel.LastName,
                    ParticipantType = createPartcipantModel.ParticipantType
                },
                IsSuccessful = true
            };
            Task<CreateParticipantResultModel> responseTask = Task.FromResult(createParticipantResultModel);


            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.AddParticipant(It.IsAny<ParticipantDomainModel>())).Returns(responseTask);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.CreateParticipantAsync(createPartcipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var createdResult = ((CreatedResult)result).Value;
            var participantDomainModel = (ParticipantDomainModel)createdResult;

            // Assert
            Assert.IsNotNull(participantDomainModel);
            Assert.AreEqual(createPartcipantModel.FirstName, participantDomainModel.FirstName);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
        }

        [TestMethod]
        public void CreateParticipantAsync_Create_Throw_DbException_Participant()
        {
            // Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            CreateParticipantModel createParticipantModel = new CreateParticipantModel()
            {
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };
            CreateParticipantResultModel createParticipantResultModel = new CreateParticipantResultModel
            {
                Participant = new ParticipantDomainModel
                {
                    Id = Guid.NewGuid(),
                    FirstName = createParticipantModel.FirstName,
                    LastName = createParticipantModel.LastName,
                    ParticipantType = createParticipantModel.ParticipantType
                },
                IsSuccessful = true
            };
            Task<CreateParticipantResultModel> responseTask = Task.FromResult(createParticipantResultModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.AddParticipant(It.IsAny<ParticipantDomainModel>())).Throws(dbUpdateException);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.CreateParticipantAsync(createParticipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            // Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void PostAsync_Create_createParticipantResultModel_IsSuccessful_False_Return_BadRequest()
        {
            // Arrange
            string expectedMessage = "Error occured while creating new participant, please try again.";
            int expectedStatusCode = 400;

            CreateParticipantModel createParticipantModel = new CreateParticipantModel()
            {
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };
            CreateParticipantResultModel createParticipantResultModel = new CreateParticipantResultModel
            {
                Participant = new ParticipantDomainModel
                {
                    Id = Guid.NewGuid(),
                    FirstName = createParticipantModel.FirstName,
                    LastName = createParticipantModel.LastName,
                    ParticipantType = createParticipantModel.ParticipantType
                },
                IsSuccessful = false,
                ErrorMessage = Messages.PARTICIPANT_CREATION_ERROR,
            };
            Task<CreateParticipantResultModel> responseTask = Task.FromResult(createParticipantResultModel);


            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.AddParticipant(It.IsAny<ParticipantDomainModel>())).Returns(responseTask);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.CreateParticipantAsync(createParticipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            // Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void CreateParticipantAsync_With_UnValid_ModelState_Return_BadRequest()
        {
            // Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            CreateParticipantModel createParticipantModel = new CreateParticipantModel()
            {
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };

            _participantService = new Mock<IParticipantService>();
            ParticipantController participantController = new ParticipantController(_participantService.Object);
            participantController.ModelState.AddModelError("key", "Invalid Model State");

            // Act
            var result = participantController.CreateParticipantAsync(createParticipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var createResult = ((BadRequestObjectResult)result).Value;
            var errorResponse = ((SerializableError)createResult).GetValueOrDefault("key");
            var message = (string[])errorResponse;

            // Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }
    }
}
