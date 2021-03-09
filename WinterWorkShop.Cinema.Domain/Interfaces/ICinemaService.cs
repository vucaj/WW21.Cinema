using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ICinemaService
    {
        Task<IEnumerable<CinemaDomainModel>> GetAllAsync();

        Task<CreateCinemaResultModel> Create(CinemaDomainModel domainModel);
    }
}
