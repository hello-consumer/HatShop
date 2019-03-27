using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
    public class CartItem
    {
        public int ID { get; set; }

        public int Quantity { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }

        public ProductColor ProductColor {get;set;}
        
        public ProductSize ProductSize { get; set; }
    }
}
