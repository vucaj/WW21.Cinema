using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Enums;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;

        public ParticipantService(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        public async Task<CreateParticipantResultModel> AddParticipant(ParticipantDomainModel newParticipant)
        {
            Participant participantToAdd = new Participant
            {
                Id = Guid.NewGuid(),
                FirstName = newParticipant.FirstName,
                LastName = newParticipant.LastName,
            };

            if(participantToAdd.ParticipantType == ParticipantType.ACTOR)
            {
                participantToAdd.ParticipantType = ParticipantType.ACTOR;
            }
            else
            {
                participantToAdd.ParticipantType = ParticipantType.DIRECTOR;
            }

            Participant insertedParticipant = _participantRepository.Insert(participantToAdd);
            if (insertedParticipant == null)
            {
                return new CreateParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PARTICIPANT_CREATION_ERROR
                };
            }
            _participantRepository.Save();

            CreateParticipantResultModel resultModel = new CreateParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Participant = new ParticipantDomainModel
                {
                    Id = insertedParticipant.Id,
                    FirstName = insertedParticipant.FirstName,
                    LastName = insertedParticipant.LastName
                }
            };

            return resultModel;
        }

        public async Task<DeleteParticipantResultModel> DeleteParticipant(ParticipantDomainModel domainModel)
        {
            var participant = await _participantRepository.GetByIdAsync(domainModel.Id);
            if (participant == null)
            {
                return new DeleteParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PARTICIPANT_NOT_FOUND
                };
            }

            _participantRepository.Delete(participant.Id);

            _participantRepository.Save();

            DeleteParticipantResultModel resultModel = new DeleteParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null
            };

            return resultModel;
        }

        public async Task<IEnumerable<ParticipantDomainModel>> GetAllParticipantsAsync()
        {
            var participant = await _participantRepository.GetAllAsync();

            return participant.Select(participant => new ParticipantDomainModel
            {
                Id = participant.Id,
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                ParticipantType = participant.ParticipantType
            });
        }

        public async Task<CreateParticipantResultModel> GetParticipantByIdAsync(ParticipantDomainModel domainModel)
        {
            var participant = await _participantRepository.GetByIdAsync(domainModel.Id);

            if (participant == null)
            {
                return new CreateParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PARTICIPANT_NOT_FOUND
                };
            }

            return new CreateParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Participant = new ParticipantDomainModel
                {
                    Id = participant.Id,
                    FirstName = participant.FirstName,
                    LastName = participant.LastName,
                    ParticipantType = participant.ParticipantType
                }
            };
        }

        public async Task<UpdateParticipantResultModel> UpdateParticipant(ParticipantDomainModel domainModel)
        {
            var participant = await _participantRepository.GetByIdAsync(domainModel.Id);

            if (participant == null)
            {
                return new UpdateParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PARTICIPANT_NOT_FOUND
                };
            }

            participant.FirstName = domainModel.FirstName;
            participant.LastName = domainModel.LastName;
            participant.ParticipantType = domainModel.ParticipantType;

            var updatedParticipant = _participantRepository.Update(participant);

            if (updatedParticipant == null)
            {
                return new UpdateParticipantResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PARTICIPANT_UPDATE_ERROR
                };
            }

            _participantRepository.Save();

            UpdateParticipantResultModel resultModel = new UpdateParticipantResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null
            };

            return resultModel;
        }
    }
}
