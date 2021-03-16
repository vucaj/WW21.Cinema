using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;

        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        /// Gets all participants

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<ParticipantDomainModel>>> GetAllAsync()
        {
            IEnumerable<ParticipantDomainModel> participantDomainModels;

            participantDomainModels = await _participantService.GetAllParticipantsAsync();

            if (participantDomainModels == null)
            {
                participantDomainModels = new List<ParticipantDomainModel>();
            }

            return Ok(participantDomainModels);
        }

        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<ParticipantDomainModel>> GetParticipantById([FromBody] ParticipantDomainModel domainModel)
        {
            var participantDomainModel = await _participantService.GetParticipantByIdAsync(new ParticipantDomainModel
            {
                Id = domainModel.Id
            });

            if (participantDomainModel == null)
            {
                return NotFound(Messages.PARTICIPANT_DOES_NOT_EXIST);
            }

            return Ok(participantDomainModel);
        }

        /// Adds a new participant

        //[Authorize(Roles = "admin")]

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ParticipantDomainModel>> CreateParticipantAsync([FromBody] CreateParticipantModel createParticipantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ParticipantDomainModel participantDomainModel = new ParticipantDomainModel
            {
                FirstName = createParticipantModel.FirstName,
                LastName = createParticipantModel.LastName,
                ParticipantType = createParticipantModel.ParticipantType
            };

            CreateParticipantResultModel createParticipantResultModel;

            try
            {
                createParticipantResultModel = await _participantService.AddParticipant(participantDomainModel);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!createParticipantResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel()
                {
                    ErrorMessage = createParticipantResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Created("participants//" + createParticipantResultModel.Participant.Id, createParticipantResultModel.Participant);
        }


        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<ParticipantDomainModel>> DeleteParticipant([FromBody] DeleteParticipantModel deleteParticipantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GetParticipantResultModel participant = await _participantService.GetParticipantByIdAsync(new ParticipantDomainModel
            {
                Id = deleteParticipantModel.ParticipantId
            });

            if (!participant.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = participant.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            ParticipantDomainModel participantDomainModel = new ParticipantDomainModel()
            {
                Id = participant.Participant.Id,
                FirstName = participant.Participant.FirstName,
                LastName = participant.Participant.LastName,
                ParticipantType = participant.Participant.ParticipantType
            };

            DeleteParticipantResultModel resultModel = await _participantService.DeleteParticipant(participantDomainModel);

            if (!resultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = resultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponseModel);
            }

            return Accepted("Participant//" + resultModel.Participant.Id, resultModel.Participant);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<ParticipantDomainModel>> UpdateParticipant([FromBody] UpdateParticipantModel updateParticipantModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var participant = await _participantService.GetParticipantByIdAsync(new ParticipantDomainModel
            {
                Id = updateParticipantModel.Id
            });

            // na ovoj liniji ispod baci error kada se pokrene test
            participant.Participant.FirstName = updateParticipantModel.FirstName;
            participant.Participant.LastName = updateParticipantModel.LastName;
            participant.Participant.ParticipantType = updateParticipantModel.ParticipantType;
            UpdateParticipantResultModel updateParticipantResultModel = await _participantService.UpdateParticipant(new ParticipantDomainModel
            {
                Id = participant.Participant.Id,
                FirstName = participant.Participant.FirstName,
                LastName = participant.Participant.LastName,
                ParticipantType = participant.Participant.ParticipantType
            });

            if (!updateParticipantResultModel.IsSuccessful)
            {
                ErrorResponseModel errorResponseModel = new ErrorResponseModel
                {
                    ErrorMessage = updateParticipantResultModel.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponseModel);
            }

            return Ok(updateParticipantResultModel);
        }
    }
}
