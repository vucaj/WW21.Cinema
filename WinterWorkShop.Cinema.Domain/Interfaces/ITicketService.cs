using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDomainModel>> GetAllAsync();

        Task<IEnumerable<TicketDomainModel>> GetByProjectionId(Guid projectionId);

        Task<IEnumerable<TicketDomainModel>> GetByUserId(Guid userId);

        Task<TicketDomainResultModel> GetById(Guid d);
    }
}