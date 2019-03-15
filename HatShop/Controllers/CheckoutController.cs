using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HatShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace HatShop.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(CheckoutViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                Console.WriteLine("Processing an order from " + model.ContactEmail);
                //TODO: Get a lot more info here, validate credit card + address, save it to a database

                return RedirectToAction("index", "receipt");
            }
            ModelState.AddModelError("SomethingElse", "We don't like you, either");
            return View(model);
        }
    }
}