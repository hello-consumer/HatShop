using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
    public class Review
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}
