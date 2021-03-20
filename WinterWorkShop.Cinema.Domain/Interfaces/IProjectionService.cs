using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IProjectionService
    {
        Task<IEnumerable<ProjectionDomainModel>> GetAllAsync();
        Task<ProjectionDomainResultModel> GetById(Guid id);
        Task<CreateProjectionResultModel> CreateProjection(ProjectionDomainModel domainModel);
        Task<DeleteProjectionResultModel> DeleteProjection(ProjectionDomainModel domainModel);
        Task<IEnumerable<ProjectionDomainModel>> GetFutureProjectionsByMovieId(Guid Id);
        Task<IEnumerable<ProjectionDomainModel>> GetFutureProjectionsByMovieId(MovieDomainModel domainModel);
    }
}
