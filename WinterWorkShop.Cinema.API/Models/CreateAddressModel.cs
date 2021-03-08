using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.API.Models
{
    public class CreateAddressModel
    {
        [MaxLength(20,ErrorMessage = Messages.ADDRESS_CITY_NAME_NOT_VALID)]
        public string CityName { get; set; }

        [MaxLength(20, ErrorMessage = Messages.ADDRESS_COUNTRY_NOT_VALID)]
        public string Country { get; set; }

        [MaxLength(50, ErrorMessage = Messages.ADDRESS_STREET_NAME_NOT_VALID)]
        public string StreetName { get; set; }

        [Range(-180.0, 180.0, ErrorMessage = Messages.ADDRESS_LONGITUDE_NOT_VALID)]
        public double Longitude { get; set; }

        [Range(-90.0, 90.0, ErrorMessage = Messages.ADDRESS_LATITUDE_NOT_VALID)]
        public double Latitude { get; set; }
    }
}
