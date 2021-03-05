using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WinterWorkShop.Cinema.Data
{
    [Table("address")]
    public class Address
    {
        [Key]
        public int Id { get; set; }
        
        [MaxLength(20)]
        public string CityName { get; set; }
        
        [MaxLength(20)]
        public string Country { get; set; }
        
        [MaxLength(50)]
        public string StreetName { get; set; }
        
        [Range(-180.0, 180.0)]
        public  double Longitude { get; set; }
        
        [Range(-90.0, 90.0)]
        public double Latitude { get; set; }
        
        public ICollection<Cinema> Cinemas { get; set; }
    }
}