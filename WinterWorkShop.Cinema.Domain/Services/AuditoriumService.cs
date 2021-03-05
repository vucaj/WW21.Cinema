using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        public Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CreateAuditoriumResultModel> CreateAuditorium(AuditoriumDomainModel domainModel, int numberOfRows, int numberOfSeats)
        {
            throw new NotImplementedException();
        }
    }
}
