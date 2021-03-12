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

        Task<CinemaDomainResultModel> GetByCinemaId(CinemaDomainModel cinemaDomainModel);

        Task<CreateCinemaResultModel> Create(CinemaDomainModel domainModel);

        Task<DeleteCinemaResultModel> Delete(CinemaDomainModel domainModel);
    }
}
