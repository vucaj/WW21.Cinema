using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IAuditoriumService
    {
        Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync();
        Task<IEnumerable<AuditoriumDomainModel>> GetAllByCinemaIdAsync(CinemaDomainModel domainModel);
        
        Task<CreateAuditoriumResultModel> CreateAuditorium(AuditoriumDomainModel domainModel, int numberOfRows, int numberOfSeats);

        // Task<DeleteAuditoriumResultModel> DeleteAuditorium(AuditoriumDomainModel domainModel);
        Task<AuditoriumDomainModel> DeleteAuditorium(Guid id);
        Task<UpdateAuditoriumResultModel> UpdateAuditorium(AuditoriumDomainModel domainModel);

        Task<AuditoriumDomainModel> FindByAuditoriumId(AuditoriumDomainModel domainModel);
    }
}
