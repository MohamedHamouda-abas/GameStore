using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BULKY.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(500)]
        public string Name { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; }


        //To Add the Company In Asp.NetUser
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public Company Company { get; set; }

    }
}
