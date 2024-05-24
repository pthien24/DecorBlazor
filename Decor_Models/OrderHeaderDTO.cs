using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decor_Models
{
    public class OrderHeaderDTO
    {
        [Key]
        public int Id { get; set; }
        
        public string UserId { get; set; }
        [Display(Name = "Order Total")]
        public double OrderTotal { get; set; }
        public DateTime OrderDate { get; set; }
        [Display(Name = "Shipping Date")]
        public DateTime ShippingDate { get; set; }
        public string Status { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set;}
        [Display(Name = "Name")]
        
        public string? Name { get; set; }
        [Display(Name = "Phone Number ")]
        
        public string? PhoneNumber { get; set; }
        [Display(Name = "Street Address")]
        
        public string? StreetAddress { get; set; }
        
        public string State { get; set; }
        
        public string City { get; set; }
        [Display(Name = "Postal Code ")]
        
        public string PostalCode { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        public string? Tracking { get; set; }
        public string? Carrier { get; set; }


    }
}
