using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class AddressServiceTests
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<ICinemasRepository> _mockCinemasRepository;
        private Address _address;
        private Data.Cinema _cinema;
        private AddressDomainModel _addressDomainModel;
        private AddressService _addressService;

        [TestInitialize]
        public void TestInitialize()
        {
            List<Address> address = new List<Address>();
            List<Data.Cinema> cinemas = new List<Data.Cinema>();

            _address = new Address
            {
                Cinemas = cinemas,
                Id = 1,
                StreetName = "",
                CityName = "",
                Country = "",
                Latitude = 1,
                Longitude = 1
            };

            _addressDomainModel = new AddressDomainModel
            {
                Id = _address.Id,
                StreetName = _address.StreetName,
                CityName = _address.CityName,
                Country = _address.Country,
                Latitude = _address.Latitude,
                Longitude = _address.Longitude
            };

            _mockAddressRepository = new Mock<IAddressRepository>();
            _addressService = new AddressService(_mockAddressRepository.Object);

        }
        //Get all Addresses test
        [TestMethod]
        public void AddressService_GetAllAddresses_ReturnListOfAddresses()
        {
            //Arrange
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            var expectedCount = 2;

            Address address2 = new Address
            {
                Id = It.IsAny<int>(),
                Cinemas = cinemas,
                CityName = "",
                StreetName = "",
                Country = "",
                Latitude = 1,
                Longitude = 1
            };

            List<Address> addresses = new List<Address>();
            addresses.Add(_address);
            addresses.Add(address2);

            List<AddressDomainModel> addressDomainModels = new List<AddressDomainModel>();
            addressDomainModels.Add(_addressDomainModel);

            _mockAddressRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(addresses);

            //Act
            var resultAction = _addressService.GetAllAddressesAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(result[0].Id, _addressDomainModel.Id);
            Assert.IsInstanceOfType(result[0], typeof(AddressDomainModel));
        }

        [TestMethod]
        public void AddressService_GetAllAddresses_Return_Empty_List()
        {
            //Arrange
            var expectedCount = 0;
            List<Address> addresses = new List<Address>();
            _mockAddressRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(addresses);

            //Act
            var resultAction = _addressService.GetAllAddressesAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
        }

        //Create Address Tests
        [TestMethod]
        public void AddressService_CreateAddress_ReturnCreatedAddress()
        {
            //Arrange
            _mockAddressRepository.Setup(x => x.Insert(It.IsAny<Address>())).Returns(_address);

            //Act
            var resultAction = _addressService.AddAddress(_addressDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_address.Id, resultAction.Address.Id);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAddressResultModel));
        }
    }
}
