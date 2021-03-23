using System;
using System.Collections.Generic;
using System.Text;

namespace WinterWorkShop.Cinema.Domain.Models
{
    public class CinemaDomainModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public int AddressId { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string StreetName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
