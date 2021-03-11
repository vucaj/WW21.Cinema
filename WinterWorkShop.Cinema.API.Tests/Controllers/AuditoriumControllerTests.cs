using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WinterWorkShop.Cinema.API.Controllers;
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
    }
}