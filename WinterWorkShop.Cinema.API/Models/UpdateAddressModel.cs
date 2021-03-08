using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.API.Models
{
    public class UpdateAddressModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string CityName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Country { get; set; }

        [Required]
        [MaxLength(50)]
        public string StreetName { get; set; }

        [Required]
        [Range(-180.0, 180.0)]
        public double Longitude { get; set; }

        [Required]
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; }
    }
}
