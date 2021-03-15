using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
    public class ParticipantServiceTests
    {
        private Mock<IParticipantRepository> _mockParticipantRespository;
        private Participant _participant;
        private ParticipantDomainModel _participantDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _participant = new Participant
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };

            _participantDomainModel = new ParticipantDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "Ime",
                LastName = "Prezime",
                ParticipantType = ParticipantType.ACTOR
            };

            List<Participant> participantModelsList = new List<Participant>();

            participantModelsList.Add(_participant);
            IEnumerable<Participant> participants = participantModelsList;
            Task<IEnumerable<Participant>> responseTask = Task.FromResult(participants);

            _mockParticipantRespository = new Mock<IParticipantRepository>();
            _mockParticipantRespository.Setup(x => x.GetAllAsync()).Returns(responseTask);
        }

/*        [TestMethod]
        public void ParticipantService_GetAllParticipantsAsync_ReturnListOfParticipants()
        {
            // Arrange
            int expectedResultCount = 1;
            ParticipantService participantController = new ParticipantService(_mockParticipantRespository.Object);

            // Act
            var resultAction = participantController.GetAllParticipantsAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = (IEnumerable<ParticipantDomainModel>)resultAction;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResultCount, result.Count);
            Assert.AreEqual(_participant.Id, result[0].Id);
            Assert.IsInstanceOfType(result[0], typeof(ParticipantDomainModel));
        }*/

        [TestMethod]
        public void ParticipantService_GetAllParticipantsAsync_ReturnNull()
        {
            // Arrange
            IEnumerable<Participant> participants = null;
            Task<IEnumerable<Participant>> responseTask = Task.FromResult(participants);

            _mockParticipantRespository = new Mock<IParticipantRepository>();
            _mockParticipantRespository.Setup(x => x.GetAllAsync()).Returns(responseTask);
            ParticipantService participantController = new ParticipantService(_mockParticipantRespository.Object);

            // Act
            var resultAction = participantController.GetAllParticipantsAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(resultAction);
        }

        // treba da se popravi
/*        [TestMethod]
        public void ParticipantService_CreateParticipant_When_Updating_Non_Existing_Participant()
        {
            // Arrange
            List<Participant> paricipantModelsList = new List<Participant>();

            _mockParticipantRespository = new Mock<IParticipantRepository>();
            _mockParticipantRespository.Setup(x => x.Insert(It.IsAny<Participant>())).Throws(new DbUpdateException());
            _mockParticipantRespository.Setup(x => x.Save());
            ParticipantService participantController = new ParticipantService(_mockParticipantRespository.Object);

            // Act
            var resultAction = participantController.AddParticipant(_participantDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            // Assert
            Assert.IsNull(resultAction);
        }*/
    }
}
