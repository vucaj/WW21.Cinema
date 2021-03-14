using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController: ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// Gets all tickets

        [HttpGet]
        [Route("getAll")]
        public async Task<ActionResult<IEnumerable<TicketDomainModel>>> GetAsync()
        {
            var ticketDomainModel = await _ticketService.GetAllAsync();

            if (ticketDomainModel == null)
            {
                return Ok(new List<TicketDomainModel>());
            }

            return Ok(ticketDomainModel);
        }
    }
}