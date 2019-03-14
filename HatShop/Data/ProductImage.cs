using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
    public class ProductImage
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string AltText { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
