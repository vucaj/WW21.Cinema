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
     public class AddressControllerTests
    {
        private Mock<IAddressService> _addressService;

        [TestMethod]
        public void GetAsync_Return_All_Addresses()
        {
            //Arrange
            List<AddressDomainModel> addressesDomainModelsList = new List<AddressDomainModel>();
            AddressDomainModel addressDomainModel = new AddressDomainModel
            {
                Id = 1,
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                StreetName = "ImeUlice",
                Latitude = 50,
                Longitude = 50
            };
            addressesDomainModelsList.Add(addressDomainModel);
            IEnumerable<AddressDomainModel> addressDomainModels = addressesDomainModelsList;
            Task<IEnumerable<AddressDomainModel>> responseTask = Task.FromResult(addressDomainModels);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.GetAllAddressesAsync()).Returns(responseTask);
            AddressController addressesController = new AddressController(_addressService.Object);

            //Act
            var result = addressesController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var addressDomainModelResultList = (List<AddressDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(addressDomainModelResultList);
            Assert.AreEqual(expectedResultCount, addressDomainModelResultList.Count);
            Assert.AreEqual(addressDomainModel.Id, addressDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void GetAsync_Return_NewList()
        {
            //Arrange
            IEnumerable<AddressDomainModel> addressDomainModels = null;
            Task<IEnumerable<AddressDomainModel>> responseTask = Task.FromResult(addressDomainModels);
            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.GetAllAddressesAsync()).Returns(responseTask);
            AddressController addressesController = new AddressController(_addressService.Object);

            //Act
            var result = addressesController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var addressDomainModelResultlist = (List<AddressDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(addressDomainModelResultlist);
            Assert.AreEqual(expectedResultCount, addressDomainModelResultlist.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void PostAsync_Create_createAddressResultModel_IsSuccessfull_True_Address()
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateAddressModel createAddressModel = new CreateAddressModel()
            {
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Longitude = 50,
                Latitude = 50
            };
            CreateAddressResultModel createAddressResultModel = new CreateAddressResultModel
            {
                Address = new AddressDomainModel
                {
                    Id = 1,
                    StreetName = createAddressModel.StreetName,
                    CityName = createAddressModel.CityName,
                    Country = createAddressModel.Country,
                    Longitude = createAddressModel.Longitude,
                    Latitude = createAddressModel.Latitude
                },
                IsSuccessful = true
            };
            Task<CreateAddressResultModel> responseTask = Task.FromResult(createAddressResultModel);

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.AddAddress(It.IsAny<AddressDomainModel>())).Returns(responseTask);
            AddressController addressesController = new AddressController(_addressService.Object);

            //Act
            var result = addressesController.CreateAddressAsync(createAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var createdResult = ((CreatedResult)result).Value;
            var addressDomainModel = (AddressDomainModel)createdResult;

            //Assert
            Assert.IsNotNull(addressDomainModel);
            Assert.AreEqual(createAddressModel.StreetName, addressDomainModel.StreetName);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
        }
        [TestMethod]
        public void PostAsync_CreateAddress_Throw_DbException_Address()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            CreateAddressModel createAddressModel = new CreateAddressModel()
            {
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Longitude = 50,
                Latitude = 50
            };
            CreateAddressResultModel createAddressResultModel = new CreateAddressResultModel()
            {
                Address = new AddressDomainModel
                {
                    Id = 1,
                    StreetName = createAddressModel.StreetName,
                    CityName = createAddressModel.CityName,
                    Country = createAddressModel.Country,
                    Longitude = createAddressModel.Longitude,
                    Latitude = createAddressModel.Latitude
                },
                IsSuccessful = true
            };
            Task<CreateAddressResultModel> responseTask = Task.FromResult(createAddressResultModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.AddAddress(It.IsAny<AddressDomainModel>())).Throws(dbUpdateException);
            AddressController addressController = new AddressController(_addressService.Object);

            //Act
            var result = addressController.CreateAddressAsync(createAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }
        [TestMethod]
        public void PostAsync_Create_createAddressResultModel_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Error occured while creating new address, please try again.";
            int expectedStatusCode = 400;

            CreateAddressModel createAddressModel = new CreateAddressModel()
            {
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 50,
                Longitude = 50
            };
            CreateAddressResultModel createAddressResultModel = new CreateAddressResultModel()
            {
                Address = new AddressDomainModel
                {
                    Id = 1,
                    StreetName = createAddressModel.StreetName,
                    CityName = createAddressModel.CityName,
                    Country = createAddressModel.Country,
                    Latitude = createAddressModel.Latitude,
                    Longitude = createAddressModel.Longitude
                },
                IsSuccessful = false,
                ErrorMessage = Messages.ADDRESS_CREATION_ERROR
            };
            Task<CreateAddressResultModel> responseTask = Task.FromResult(createAddressResultModel);

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.AddAddress(It.IsAny<AddressDomainModel>())).Returns(responseTask);
            AddressController addressController = new AddressController(_addressService.Object);

            //Act
            var result = addressController.CreateAddressAsync(createAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }
        [TestMethod]
        public void PostAsync_With_Invalid_ModelState_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            CreateAddressModel createAddressModel = new CreateAddressModel()
            {
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 50,
                Longitude = 50
            };

            _addressService = new Mock<IAddressService>();
            AddressController addressesController = new AddressController(_addressService.Object);
            addressesController.ModelState.AddModelError("key", "Invalid Model State");

            //Act
            var result = addressesController.CreateAddressAsync(createAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var createdResult = resultResponse.Value;
            var errorResponse = ((SerializableError)createdResult).GetValueOrDefault("key");
            var message = (string[])errorResponse;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }
        [TestMethod]
        public void PostAsync_DeleteAddress_Address()
        {
            //Arrange
            List<AddressDomainModel> addressDomainModelsList = new List<AddressDomainModel>();

            AddressDomainModel addressDomainModels = new AddressDomainModel
            {
                Id = It.IsAny<int>(),
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 50,
                Longitude = 50
            };

            CreateAddressResultModel addressDomainResultModel = new CreateAddressResultModel()
            {
                IsSuccessful = true,
                Address = addressDomainModels,
                ErrorMessage = null
            };

            addressDomainModelsList.Add(addressDomainModels);

            DeleteAddressModel deleteAddressModel = new DeleteAddressModel()
            {
                AddressId = addressDomainModels.Id
            };

            DeleteAddressResultModel deleteAddressResultModel = new DeleteAddressResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Address = new AddressDomainModel()
                {
                    Id = addressDomainModels.Id,
                    StreetName = "ImeUlice",
                    CityName = "ImeGrada",
                    Country = "ImeDrzave",
                    Latitude = 50,
                    Longitude = 50
                }
            };

            Task<DeleteAddressResultModel> responseTask = Task.FromResult(deleteAddressResultModel);
            Task<CreateAddressResultModel> responseTask2 = Task.FromResult(addressDomainResultModel);

            int expectedStatusCode = 202;

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.DeleteAddress(It.IsAny<AddressDomainModel>())).Returns(responseTask);
            _addressService.Setup(x => x.GetAddressByIdAsync(It.IsAny<AddressDomainModel>())).Returns(responseTask2);
            AddressController addressController = new AddressController(_addressService.Object);

            //Act
            var result = addressController.DeleteAddress(deleteAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var deleteResult = ((AcceptedResult)result).Value;
            var addressDomainModel = (AddressDomainModel)deleteResult;

            //Assert
            Assert.IsNotNull(addressDomainModel);
            Assert.AreEqual(deleteAddressModel.AddressId, addressDomainModel.Id);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
        }
        [TestMethod]
        public void PostAsync_DeleteAddress_BadRequest()
        {
            //Arrange
            string expectedMessage = Messages.ADDRESS_NOT_FOUND;
            int expectedStatusCode = 400;

            List<AddressDomainModel> addressDomainModelsList = new List<AddressDomainModel>();

            AddressDomainModel addressDomainModels = new AddressDomainModel
            {
                Id = It.IsAny<int>(),
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 50,
                Longitude = 50
            };

            addressDomainModelsList.Add(addressDomainModels);

            DeleteAddressResultModel deleteAddressResultModel = new DeleteAddressResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.ADDRESS_NOT_FOUND,
                Address = null
            };

            CreateAddressResultModel addressDomainResultModel = new CreateAddressResultModel()
            {
                IsSuccessful = false,
                Address = null,
                ErrorMessage = Messages.ADDRESS_NOT_FOUND
            };

            DeleteAddressModel deleteAddressModel = new DeleteAddressModel()
            {
                AddressId = It.IsAny<int>()
            };

            Task<DeleteAddressResultModel> responseTask = Task.FromResult(deleteAddressResultModel);
            Task<CreateAddressResultModel> responseTask2 = Task.FromResult(addressDomainResultModel);

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.DeleteAddress(addressDomainModels)).Returns(responseTask);
            _addressService.Setup(x => x.GetAddressByIdAsync(It.IsAny<AddressDomainModel>())).Returns(responseTask2);
            AddressController addressController = new AddressController(_addressService.Object);

            //Act
            var result = addressController.DeleteAddress(deleteAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }
        [TestMethod]
        public void GetAddressById_Return_Participant()
        {
            // Arrange
            AddressDomainModel addressDomdainModel = new AddressDomainModel
            {
                Id = It.IsAny<int>(),
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 1,
                Longitude = 1
            };

            CreateAddressResultModel createAddressResultModel = new CreateAddressResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Address = new AddressDomainModel
                {
                    Id = addressDomdainModel.Id,
                    StreetName = addressDomdainModel.StreetName,
                    CityName = addressDomdainModel.CityName,
                    Country = addressDomdainModel.Country,
                    Longitude = addressDomdainModel.Longitude,
                    Latitude = addressDomdainModel.Latitude
                }
            };

            Task<CreateAddressResultModel> responseTask = Task.FromResult(createAddressResultModel);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.GetAddressByIdAsync(It.IsAny<AddressDomainModel>())).Returns(responseTask);
            AddressController addressController = new AddressController(_addressService.Object);

            // Act
            var result = addressController.GetAddressByIdAsync(addressDomdainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var addressDomainModelResultList = (CreateAddressResultModel)resultList;

            // Assert
            Assert.IsNotNull(addressDomainModelResultList);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAddressById_WhenAddresstIsNull_ReturnsNotFound_Tests()
        {
            // Arrange
            AddressDomainModel addressDomainModel = new AddressDomainModel
            {
                Id = It.IsAny<int>(),
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 1,
                Longitude = 1
            };

            CreateAddressResultModel createAddressResultModel = new CreateAddressResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.ADDRESS_NOT_FOUND,
                Address = addressDomainModel
            };

            Task<CreateAddressResultModel> responseTask = Task.FromResult(createAddressResultModel);
            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.GetAddressByIdAsync(addressDomainModel)).Returns(responseTask);

            AddressController addressController = new AddressController(_addressService.Object);
            int expectedStatusCode = 404;

            // Act
            var test = addressController.GetAddressByIdAsync(addressDomainModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;


            // Assert
            Assert.IsInstanceOfType(test, typeof(NotFoundResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundResult)test).StatusCode);
        }
        [TestMethod]
        public void PostAsync_UpdateAddress_Address()
        {
            // Arrange
            List<AddressDomainModel> addressDomainModelsList = new List<AddressDomainModel>();

            AddressDomainModel addressDomainModel = new AddressDomainModel
            {
                Id = It.IsAny<int>(),
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 1,
                Longitude = 1

            };

            CreateAddressResultModel createAddressResultModel = new CreateAddressResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Address = addressDomainModel
            };

            addressDomainModelsList.Add(addressDomainModel);

            UpdateAddressModel updateAddressModel = new UpdateAddressModel()
            {
                Id = addressDomainModel.Id
            };

            UpdateAddressResultModel updateAddressResultModel = new UpdateAddressResultModel()
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Address = new AddressDomainModel
                {
                    Id = addressDomainModel.Id,
                    StreetName = addressDomainModel.StreetName,
                    CityName = addressDomainModel.CityName,
                    Country = addressDomainModel.Country,
                    Latitude = addressDomainModel.Latitude,
                    Longitude = addressDomainModel.Longitude
                }
            };

            Task<UpdateAddressResultModel> responseTask = Task.FromResult(updateAddressResultModel);
            Task<CreateAddressResultModel> responseTask2 = Task.FromResult(createAddressResultModel);

            int expectedStatusCode = 200;

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.UpdateAddress(It.IsAny<AddressDomainModel>())).Returns(responseTask);
            _addressService.Setup(x => x.GetAddressByIdAsync(It.IsAny<AddressDomainModel>())).Returns(responseTask2);
            AddressController addressController = new AddressController(_addressService.Object);

            // Act
            var result = addressController.UpdateAddress(updateAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var updateResult = ((OkObjectResult)result).Value;
            var addressDomainModels = (UpdateAddressResultModel)updateResult;

            // Assert
            Assert.IsNotNull(addressDomainModels);
            Assert.AreEqual(updateAddressModel.Id, addressDomainModels.Address.Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void UpdateAddressAsync_With_Invalid_ModelState_Return_BadRequest()
        {
            // Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            UpdateAddressModel updateAddressModel = new UpdateAddressModel()
            {
                Id = It.IsAny<int>(),
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 1,
                Longitude = 1
            };

            _addressService = new Mock<IAddressService>();
            AddressController addressController = new AddressController(_addressService.Object);
            addressController.ModelState.AddModelError("key", "Invalid Model State");

            // Act
            var result = addressController.UpdateAddress(updateAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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

        //not working
    /*    [TestMethod]
        public void PostAsync_UpdateAddress_BadRequest()
        {
            // Arrange
            string expectedMessage = "Error occured while updating address.";
            int expectedStatusCode = 400;

            UpdateAddressModel updateAddressModel = new UpdateAddressModel()
            {
                Id = It.IsAny<int>(),
                StreetName = "ImeUlice",
                CityName = "ImeGrada",
                Country = "ImeDrzave",
                Latitude = 1,
                Longitude = 1
            };

            UpdateAddressResultModel updateAddressResultModel = new UpdateAddressResultModel()
            {
                Address = null,
                IsSuccessful = false,
                ErrorMessage = Messages.ADDRESS_UPDATE_ERROR
            };

            CreateAddressResultModel createAddressResultModel = new CreateAddressResultModel()
            {
                IsSuccessful = false,
                ErrorMessage = Messages.ADDRESS_UPDATE_ERROR,
                Address = null
            };

            Task<UpdateAddressResultModel> responseTask = Task.FromResult(updateAddressResultModel);
            Task<CreateAddressResultModel> responseTask1 = Task.FromResult(createAddressResultModel);

            _addressService = new Mock<IAddressService>();
            _addressService.Setup(x => x.GetAddressByIdAsync(It.IsAny<AddressDomainModel>())).Returns(responseTask1);
            _addressService.Setup(x => x.UpdateAddress(It.IsAny<AddressDomainModel>())).Returns(responseTask);
            AddressController addressController = new AddressController(_addressService.Object);


            // Act
                    // Object reference not set to an instance of an object.
            var result = addressController.UpdateAddress(updateAddressModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            // Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        } */
    }
}
