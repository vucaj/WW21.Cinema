using System;
using System.ComponentModel.DataAnnotations;

namespace WinterWorkShop.Cinema.API.Models
{
    public class DeleteCinemaModel
    {
        [Required]
        public Guid Id { get; set; }
    }
}