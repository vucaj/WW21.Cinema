using System;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class DeleteCinemaResultModel
    {
        public bool isSuccessful { get; set; }

        public string ErrorMessage { get; set; }
    }
}