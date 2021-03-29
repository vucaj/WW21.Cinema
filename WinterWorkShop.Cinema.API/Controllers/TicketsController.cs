using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
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
        
        
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<IEnumerable<TicketDomainModel>>> CreateTicketAsync(
            [FromBody] CreateTicketModelList createTicketModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ticketDomainModels = new List<TicketDomainModel>();
            
            foreach (var tdm in createTicketModel.CreateTicketModels)
            {
                TicketDomainModel ticketDomainModel = new TicketDomainModel()
                {
                    ProjectionId = tdm.ProjectionId,
                    SeatId = tdm.SeatId,
                    UserId = tdm.UserId
                };

                CreateTicketDomainResultModel createTicketDomainResultModel;

                try
                {
                    createTicketDomainResultModel = await _ticketService.CreateTicket(ticketDomainModel);
                }
                catch (DbUpdateException e)
                {
                    ErrorResponseModel errorResponseModel = new ErrorResponseModel()
                    {
                        ErrorMessage = e.InnerException.Message ?? e.Message,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };

                    return BadRequest(errorResponseModel);
                }

                if (!createTicketDomainResultModel.IsSuccessful)
                {
                    ErrorResponseModel errorResponseModel = new ErrorResponseModel()
                    {
                        ErrorMessage = createTicketDomainResultModel.ErrorMessage,
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };

                    return BadRequest(errorResponseModel);
                }
                
                ticketDomainModels.Add(new TicketDomainModel()
                {
                    Id = createTicketDomainResultModel.Ticket.Id,
                    ProjectionId = createTicketDomainResultModel.Ticket.ProjectionId,
                    SeatId = createTicketDomainResultModel.Ticket.SeatId,
                    UserId = createTicketDomainResultModel.Ticket.UserId
                });
            }

            return Created("Tickets", ticketDomainModels);
        }
    }
}