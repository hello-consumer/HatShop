using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Models
{

    //I use a View Model to define the "shape" of the data that I'm collecting on the Checkout page.
    //While I'll eventually use this to make up "order" data, I'm using a View Model to add some custom rules
    //and to hide some of the order implementation details.
    public class CheckoutViewModel
    {
        //using System.ComponentModel.DataAnnotations;
        [Required(ErrorMessage = "We need your name")]
        [Display(Name = "Your Name")]   //This changes the generated label - this can also be localized: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/portable-object-localization?view=aspnetcore-2.2
        public string ContactName { get; set; }

        [Display(Name = "Your Email")]
        [EmailAddress]
        public string ContactEmail { get; set; }

        [Phone]
        public string ContactPhoneNumber { get; set; }

        [Required]
        public string ShippingStreet1 { get; set; }

        public string ShippingStreet2 { get; set; }

        [Required]
        public string ShippingCity { get; set; }

        [Required]
        public string ShippingRegion { get; set; }

        [Required]
        public string ShippingCountry { get; set; }

        [Required]
        public string ShippingPostalCode { get; set; }

    }
}
