using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateCinemaModel
    {
        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 2, ErrorMessage = Messages.CINEMA_PROPERTY_NAME_NOT_VALID)]
        public string Name { get; set; }
        
        [Required]
        public int AddressId { get; set; }
    }
}