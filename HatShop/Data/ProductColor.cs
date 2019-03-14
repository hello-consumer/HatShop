using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
    public class ProductColor
    {
        public int ID { get; set; }
        public string Color { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
