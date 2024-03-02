using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BULKY.Models.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(500)]
        public string? StreetAddress { get; set; }
        [MaxLength(500)]
        public string? City { get; set; }
        [MaxLength(500)]
        public string? State { get; set; }
        [MaxLength(500)]
        public string? PostalCode { get; set; }     
        public int? PhoneNumber { get; set; }
    }
}
