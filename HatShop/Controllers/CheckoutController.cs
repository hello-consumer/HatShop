using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HatShop.Data;
using HatShop.Models;
using HatShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;

namespace HatShop.Controllers
{
    public class CheckoutController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<HatUser> _userManager;
        private IEmailSender _emailSender;

        public CheckoutController(ApplicationDbContext context, UserManager<HatUser> userManager
        , IEmailSender emailSender)
        {
            this._context = context;
            this._userManager = userManager;
            this._emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {

            if (this.ModelState.IsValid)
            {
                HatUser hatUser = null;
                if (User.Identity.IsAuthenticated)
                {
                    hatUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
                }
                Cart cart = CartService.GetCart(_context, Request, Response, hatUser);

                if (cart.CartItems.Count > 0)
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


                    order.OrderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        ProductID = ci.ProductID,
                        Color = ci.ProductColor != null ? ci.ProductColor.Color : null,
                        Description = ci.Product.Description,
                        Name = ci.Product.Name,
                        Price = ci.Product.Price,
                        Quantity = ci.Quantity,
                        Size = ci.ProductSize != null ? ci.ProductSize.Size : null
                    }).ToArray();

                    _context.CartItems.RemoveRange(cart.CartItems);
                    _context.Carts.Remove(cart);
                    Response.Cookies.Delete("HatShopCartInfo");
                    _context.Orders.Add(order);
                    if (hatUser != null)
                    {
                        order.HatUser = hatUser;
                    }

                    _context.SaveChanges();
                    await _emailSender.SendEmailAsync(model.ContactEmail, "Receipt for order #" + order.ID, "Thanks for your order!");

                    return RedirectToAction("index", "receipt", new { id = order.ID });
                }
                ModelState.AddModelError("cart", "There was a problem processing your cart");


            }
            return View(model);
        }
    }
}