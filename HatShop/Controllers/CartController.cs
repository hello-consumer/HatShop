using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HatShop.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {

            string cookieData = Request.Cookies.ContainsKey("HatShopCartInfo") ? Request.Cookies["HatShopCartInfo"] : null;
            if (!string.IsNullOrEmpty(cookieData))
            {
                string[] parts = cookieData.Split("|");
                int productId = int.Parse(parts[0]);
                int colorId = int.Parse(parts[1]);
                int sizeId = int.Parse(parts[2]);
            }

            return View();
        }
    }
}