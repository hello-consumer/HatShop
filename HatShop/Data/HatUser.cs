using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Data
{
    public class HatUser : IdentityUser
    {
        public HatUser() : base()
        {
            this.Reviews = new HashSet<Review>();
        }

        public HatUser(string userName) : base(userName)
        {
            this.Reviews = new HashSet<Review>();
        }

        //Individual values
        public decimal? HatSize { get; set; }

        //Relationships
        public ICollection<Review> Reviews { get; set; }

        public Cart Cart { get; set; }

        public int? CartID { get; set; }

    }
}
