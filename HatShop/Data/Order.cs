using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
    public class Order
    {
        public Order()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        public string ID { get; set; }
        public string OrderDate { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        public string ShippingStreet1 { get; set; }
        public string ShippingStreet2 { get; set; }
        public string ShippingCity { get; set; }
        public string ShippingRegion { get; set; }
        public string ShippingCountry { get; set; }
        public string ShippingPostalCode { get; set; }

        public HatUser HatUser { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
