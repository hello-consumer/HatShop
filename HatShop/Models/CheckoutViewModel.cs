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
        [RegularExpression(@"^([\+]?[0-9]{1,3}[\s.-][0-9]{1,12})([\s.-]?[0-9]{1,4}?)$")]
        public string ContactPhoneNumber { get; set; }

        public string ShippingStreet1 { get; set; }

        public string ShippingStreet2 { get; set; }

        public string ShippingCity { get; set; }

        public string ShippingRegion { get; set; }

        public string ShippingLocale { get; set; }

        public string ShippingCountry { get; set; }

        public string ShippingPostalCode { get; set; }

    }
}
