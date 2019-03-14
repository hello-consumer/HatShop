using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
    public class Product
    {
        public Product()
        {
            this.ProductImages = new HashSet<ProductImage>();
            this.ProductSizes = new HashSet<ProductSize>();
            this.ProductColors = new HashSet<ProductColor>();
            this.Reviews = new HashSet<Review>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductSize> ProductSizes { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
