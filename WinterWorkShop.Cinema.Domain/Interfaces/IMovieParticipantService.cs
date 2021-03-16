using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IMovieParticipantService
    {
        Task<IEnumerable<MovieParticipantDomainModel>> GetAllAsync();

        Task<GetMovieParticipantResultModel> GetByMovieParticipantId(MovieParticipantDomainModel domainModel);

        Task<CreateMovieParticipantResultModel> Create(MovieParticipantDomainModel newParticipant);

        Task<DeleteMovieParticipantResultModel> Delete(MovieParticipantDomainModel deleteParticipant);

        Task<UpdateMovieParticipantResultModel> Update(MovieParticipantDomainModel updateParticipant);

        Task<IEnumerable<GetMovieParticipantResultModel>> GetAllByMovieIdAsync(MovieDomainModel domainModel);

        Task<IEnumerable<GetMovieParticipantResultModel>> GetAllByParticipantIdAsync(ParticipantDomainModel domainModel);

        Task<GetMovieParticipantResultModel> GetByParticipantIdAsync(Guid id);

        /*Task<DeleteMovieResultModel> GetByMovieIdAsync(Guid id);*/
    }
}
