using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
    public class OrderItem
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
