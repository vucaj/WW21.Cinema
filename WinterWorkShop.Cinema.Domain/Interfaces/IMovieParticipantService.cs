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

        Task<CreateMovieParticipantResultModel> GetByMovieParticipantId(MovieParticipantDomainModel domainModel);

        Task<CreateMovieParticipantResultModel> Create(MovieParticipantDomainModel domainModel);

        Task<DeleteMovieParticipantResultModel> Delete(MovieParticipantDomainModel domainModel);

        Task<UpdateMovieParticipantResultModel> Update(MovieParticipantDomainModel domainModel);

        Task<IEnumerable<CreateMovieParticipantResultModel>> GetAllByMovieIdAsync(MovieDomainModel domainModel);

        Task<IEnumerable<CreateMovieParticipantResultModel>> GetAllByParticipantIdAsync(ParticipantDomainModel domainModel);

        Task<DeleteParticipantResultModel> GetByParticipantIdAsync(Guid id);

        /*Task<DeleteMovieResultModel> GetByMovieIdAsync(Guid id);*/
    }
}
