using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HatShop.Data;
using Microsoft.AspNetCore.Mvc;

namespace HatShop.Controllers
{
    

    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View(MockShopData.categories);
        }

        public IActionResult Details(int? id)
        {
            Product productToFind = null;
            foreach(var category in MockShopData.categories)
            {
                foreach(var product in category.Products)
                {
                    if(product.ID == id)
                    {
                        productToFind = product;
                    }
                }
            }
            
            return View(productToFind);
        }

        [HttpPost]
        public IActionResult Details(int id, int color, int size)
        {
            //TODO: I need to add a bunch of other "logic" here to add the item to a cart
            Response.Cookies.Append("HatShopCartInfo", id + "|" + color + "|" + size);

            return RedirectToAction("Index", "Cart", new { moreData = "More Tacos" });
        }
    }
}