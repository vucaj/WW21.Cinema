using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class UpdateAddressResultModel
    {
        public bool IsSuccessful{get; set;}
        public string ErrorMessage{get; set;}
        public AddressDomainModel Address { get; set; }
    }
}
