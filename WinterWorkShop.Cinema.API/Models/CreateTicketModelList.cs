using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateTicketModelList
    {
        [Required]
        public IEnumerable<CreateTicketModel> CreateTicketModels { get; set; }
        
        [Required]
        public Guid UserId { get; set; }
        
        [Required]
        public Guid ProjectionId { get; set; }
    }
}