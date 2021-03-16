using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateTicketModelList
    {
        [Required]
        public List<CreateTicketModel> CreateTicketModels { get; set; }
    }
}