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

        [TestMethod]
        public void GetParticipantById_Return_Participant()
        {
            // Arrange
            ParticipantDomainModel participantDomainModel = new ParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };

            GetParticipantResultModel getParticipantResultModel = new GetParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Participant = new ParticipantDomainModel
                {
                    Id = participantDomainModel.Id,
                    FirstName = participantDomainModel.FirstName,
                    LastName = participantDomainModel.LastName,
                    ParticipantType = participantDomainModel.ParticipantType
                }
            };

            Task<GetParticipantResultModel> responseTask = Task.FromResult(getParticipantResultModel);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.GetParticipantByIdAsync(It.IsAny<ParticipantDomainModel>())).Returns(responseTask);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.GetParticipantById(participantDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var participantDomainModelResultList = (GetParticipantResultModel)resultList;

            // Assert
            Assert.IsNotNull(participantDomainModelResultList);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
              
        [TestMethod]
        public async Task GetParticipantById_WhenParticipantIsNull_ReturnsNotFound_Tests()
        {
            // Arrange
            ParticipantDomainModel participantDomainModel = new ParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };

            GetParticipantResultModel getParticipantResultModel = new GetParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Participant = new ParticipantDomainModel
                {
                    Id = participantDomainModel.Id,
                    FirstName = participantDomainModel.FirstName,
                    LastName = participantDomainModel.LastName,
                    ParticipantType = participantDomainModel.ParticipantType
                }
            };

            Task<GetParticipantResultModel> responseTask = Task.FromResult(getParticipantResultModel);
            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.GetParticipantByIdAsync(participantDomainModel)).Returns(responseTask);

            var participantController = new ParticipantController(_participantService.Object);

            // Act
            var test = await participantController.GetParticipantById(participantDomainModel);

            // Assert
            Assert.IsInstanceOfType(test.Result, typeof(NotFoundObjectResult));
        }

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

        [TestMethod]
        public void PostAsync_DeleteParticipant_Participant()
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

            GetParticipantResultModel getParticipantResultModel = new GetParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Participant = participantDomainModel
            };

            participantDomainModelsList.Add(participantDomainModel);

            DeleteParticipantModel deleteParticipantModel = new DeleteParticipantModel()
            {
                ParticipantId = participantDomainModel.Id
            };

            DeleteParticipantResultModel deleteParticipantResultModel = new DeleteParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Participant = new ParticipantDomainModel()
                {
                    Id = participantDomainModel.Id,
                    FirstName = "Ime",
                    LastName = "Prezime",
                    ParticipantType = ParticipantType.ACTOR
                }
            };

            Task<DeleteParticipantResultModel> responseTask = Task.FromResult(deleteParticipantResultModel);
            Task<GetParticipantResultModel> responseTask2 = Task.FromResult(getParticipantResultModel);

            int expectedStatusCode = 202;

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.DeleteParticipant(It.IsAny<ParticipantDomainModel>())).Returns(responseTask);
            _participantService.Setup(x => x.GetParticipantByIdAsync(It.IsAny<ParticipantDomainModel>())).Returns(responseTask2);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.DeleteParticipant(deleteParticipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var deletedResult = ((AcceptedResult)result).Value;
            var participantDomainModel1 = (ParticipantDomainModel)deletedResult;

            // Assert
            Assert.IsNotNull(participantDomainModel1);
            Assert.AreEqual(deleteParticipantModel.ParticipantId, participantDomainModel1.Id);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_DeleteParticipant_BadRequest()
        {
            // Arrange
            string expectedMessage = Messages.PARTICIPANT_NOT_FOUND;
            int expectedStatusCode = 400;

            List<ParticipantDomainModel> participantDomainModelsList = new List<ParticipantDomainModel>();

            ParticipantDomainModel participantDomainModel1 = new ParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };

            participantDomainModelsList.Add(participantDomainModel1);

            DeleteParticipantResultModel deleteParticipantResultModel = new DeleteParticipantResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.PARTICIPANT_NOT_FOUND,
                Participant = null
            };

            GetParticipantResultModel participantDomainResultModel = new GetParticipantResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.PARTICIPANT_NOT_FOUND,
                Participant = null
            };

            DeleteParticipantModel deleteParticipantModel = new DeleteParticipantModel()
            {
                ParticipantId = Guid.NewGuid()
            };

            Task<DeleteParticipantResultModel> responseTask = Task.FromResult(deleteParticipantResultModel);
            Task<GetParticipantResultModel> responseTaks2 = Task.FromResult(participantDomainResultModel);

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.DeleteParticipant(participantDomainModel1)).Returns(responseTask);
            _participantService.Setup(x => x.GetParticipantByIdAsync(It.IsAny<ParticipantDomainModel>())).Returns(responseTaks2);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.DeleteParticipant(deleteParticipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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
        public void PostAsync_UpdateParticipant_Participant()
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

            GetParticipantResultModel getParticipantResultModel = new GetParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Participant = participantDomainModel
            };

            participantDomainModelsList.Add(participantDomainModel);

            UpdateParticipantModel updateParticipantModel = new UpdateParticipantModel()
            {
                Id = participantDomainModel.Id
            };

            UpdateParticipantResultModel updateParticipantResultModel = new UpdateParticipantResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Participant = new ParticipantDomainModel
                {
                    Id = participantDomainModel.Id,
                    FirstName = participantDomainModel.FirstName,
                    LastName = participantDomainModel.LastName,
                    ParticipantType = participantDomainModel.ParticipantType
                }
            };

            Task<UpdateParticipantResultModel> responseTask = Task.FromResult(updateParticipantResultModel);
            Task<GetParticipantResultModel> responseTask2 = Task.FromResult(getParticipantResultModel);

            int expectedStatusCode = 200;

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.UpdateParticipant(It.IsAny<ParticipantDomainModel>())).Returns(responseTask);
            _participantService.Setup(x => x.GetParticipantByIdAsync(It.IsAny<ParticipantDomainModel>())).Returns(responseTask2);
            ParticipantController participantController = new ParticipantController(_participantService.Object);

            // Act
            var result = participantController.UpdateParticipant(updateParticipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var updateResult = ((OkObjectResult)result).Value;
            var participantDomainModel1 = (UpdateParticipantResultModel)updateResult;

            // Assert
            Assert.IsNotNull(participantDomainModel1);
            Assert.AreEqual(updateParticipantModel.Id, participantDomainModel1.Participant.Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void UpdateParticipantAsync_With_UnValid_ModelState_Return_BadRequest()
        {
            // Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            UpdateParticipantModel updateParticipantModel = new UpdateParticipantModel()
            {
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };

            _participantService = new Mock<IParticipantService>();
            ParticipantController participantController = new ParticipantController(_participantService.Object);
            participantController.ModelState.AddModelError("key", "Invalid Model State");

            // Act
            var result = participantController.UpdateParticipant(updateParticipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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

        /*[TestMethod]
        public void PostAsync_UpdateParticipant_BadRequest()
        {
            // Arrange
            string expectedMessage = "Error occured while updating participant.";
            int expectedStatusCode = 400;

            UpdateParticipantModel updateParticipantModel = new UpdateParticipantModel()
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };

            UpdateParticipantResultModel updateParticipantResultModel = new UpdateParticipantResultModel()
            {
                Participant = null,
                IsSuccessful = false,
                ErrorMessage = Messages.PARTICIPANT_UPDATE_ERROR
            };

            GetParticipantResultModel getParticipantResultModel = new GetParticipantResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.PARTICIPANT_NOT_FOUND,
                Participant = null
            };

            Task<UpdateParticipantResultModel> responseTask = Task.FromResult(updateParticipantResultModel);
            Task<GetParticipantResultModel> responseTask1 = Task.FromResult(getParticipantResultModel);

            _participantService = new Mock<IParticipantService>();
            _participantService.Setup(x => x.GetParticipantByIdAsync(It.IsAny<ParticipantDomainModel>())).Returns(responseTask1);
            _participantService.Setup(x => x.UpdateParticipant(It.IsAny<ParticipantDomainModel>())).Returns(responseTask);
            ParticipantController participantController = new ParticipantController(_participantService.Object);


            // Act
            // Object reference not set to an instance of an object.
            var result = participantController.UpdateParticipant(updateParticipantModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            // Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }*/
    }
}
