using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class ProjectionService : IProjectionService
    {
        public Task<IEnumerable<ProjectionDomainModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CreateProjectionResultModel> CreateProjection(ProjectionDomainModel domainModel)
        {
            throw new NotImplementedException();
        }
    }
}
