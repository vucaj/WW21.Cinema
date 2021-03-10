using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        /// <summary>
        /// Gets all seats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetAsync()
        {
            IEnumerable<SeatDomainModel> seatDomainModels;

            seatDomainModels = await _seatService.GetAllAsync();

            if (seatDomainModels == null)
            {
                seatDomainModels = new List<SeatDomainModel>();
            }

            return Ok(seatDomainModels);
        }
        [HttpGet]
        [Route("getById")]
        public async Task<ActionResult<SeatDomainModel>>GetByIdAsync([FromBody]SeatDomainModel domainModel)
        {
            var seatById = await _seatService.GetByIdAsync(new SeatDomainModel 
            { 
                Id = domainModel.Id
            });

            if (seatById == null)
            {
                return NotFound();
            }

            return Ok(seatById);
        }
        [HttpGet]
        [Route("getByAuditId")]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetAllByAuditoriumIdAsync([FromBody]AuditoriumDomainModel domainModel)
        {
            var seatByAuditId = await _seatService.GetAllByAuditoriumIdAsync(new AuditoriumDomainModel 
            {
                Id = domainModel.Id
            });

            if (seatByAuditId == null)
            {
                return NotFound();
            }

            return Ok(seatByAuditId);
        }

        [HttpGet]
        [Route("getBySeatType")]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetBySeatType([FromBody] SeatDomainModel domainModel )
        {
            var seatBySeatType = await _seatService.GetByAllSeatTypeAsync(new SeatDomainModel
            {
                SeatType = domainModel.SeatType
            });

            if(seatBySeatType == null)
            {
                return NotFound();
            }

            return Ok(seatBySeatType);
        }
    }
}
