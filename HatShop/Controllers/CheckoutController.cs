using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HatShop.Data;
using HatShop.Models;
using Microsoft.AspNetCore.Mvc;

namespace HatShop.Controllers
{
    public class CheckoutController : Controller
    {
        private ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(CheckoutViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                //TODO: Get a lot more info here, validate credit card + address, save it to a database

                Order order = new Order();
                order.ID = Guid.NewGuid().ToString().Substring(0, 8);
                order.OrderDate = DateTime.Now.ToString();
                order.ContactEmail = model.ContactEmail;
                order.ContactName = model.ContactName;
                order.ContactPhoneNumber = model.ContactPhoneNumber;
                order.ShippingCity = model.ShippingCity;
                order.ShippingCountry = model.ShippingCountry;
                order.ShippingPostalCode = model.ShippingPostalCode;
                order.ShippingRegion = model.ShippingRegion;
                order.ShippingStreet1 = model.ShippingStreet1;
                order.ShippingStreet2 = model.ShippingStreet2;
                _context.Orders.Add(order);
                _context.SaveChanges();

                return RedirectToAction("index", "receipt", new { id = order.ID });
            }
            return View(model);
        }
    }
}