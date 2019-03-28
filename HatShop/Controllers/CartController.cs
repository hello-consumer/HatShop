using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HatShop.Data;
using HatShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HatShop.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            Cart cart = CartService.GetCart(Request, _context, Response);
            return View(cart);
        }

        [HttpPost]
        public IActionResult Index(int cartItemId, int quantity)
        {
            CartItem cartItem = _context.CartItems.Find(cartItemId);
            cartItem.Quantity = quantity;

            if(quantity == 0)
            {
                _context.CartItems.Remove(cartItem);
            }

            _context.SaveChanges();


            return RedirectToAction("index");
        }

        public IActionResult Count()
        {
            Cart cart = CartService.GetCart(Request, _context, Response);
            return Json(cart.CartItems.Sum(x => x.Quantity));
        }
    }
}