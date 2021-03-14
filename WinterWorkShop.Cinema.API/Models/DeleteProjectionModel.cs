using System;
using System.ComponentModel.DataAnnotations;

namespace WinterWorkShop.Cinema.API.Models
{
    public class DeleteProjectionModel
    {
        [Required]
        public Guid ProjectionId { get; set; }
    }
}