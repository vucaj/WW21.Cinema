using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IParticipantService
    {
        Task<IEnumerable<ParticipantDomainModel>> GetAllParticipantsAsync();
        Task<ParticipantDomainModel> GetParticipantByIdAsync(ParticipantDomainModel domainModel);
        Task<CreateParticipantResultModel> AddParticipant(ParticipantDomainModel newParticipant);
        Task<UpdateParticipantResultModel> UpdateParticipant(ParticipantDomainModel updateParticipant);
        Task<DeleteParticipantResultModel> DeleteParticipant(ParticipantDomainModel deleteParticipant);
    }
}
