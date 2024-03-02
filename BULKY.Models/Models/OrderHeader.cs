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
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderTotal { get; set; }

        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }

        //Company
        public string? TrackingNumber { get; set; }
        public string? Carrier { get; set; }

        //If SessionId is true the PaymentIntentId will be created
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        //Data Info
        [Required, MaxLength(500)]
        public string Name { get; set; }
        [Required, MaxLength(500)]
        public string? City { get; set; }
        [Required, MaxLength(500)]
        public string? State { get; set; }
        [Required, MaxLength(500)]
        public string? StreetAddress { get; set; }
        [Required, MaxLength(500)]
        public string? PostalCode { get; set; }
        [Required, MaxLength(500)]
        public string? PhoneNumber { get; set; }
    }
}
