using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace HatShop.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int? id)
        {
            ViewData["ID"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult Details()
        {
            //TODO: I need to add a bunch of other "logic" here to add the item to a cart
            return RedirectToAction("Index", "Cart", new { moreData = "More Tacos" });
        }
    }
}