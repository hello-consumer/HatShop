using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HatShop.Data;
using HatShop.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HatShop.Controllers
{
    public class CartController : Controller
    {
        private ApplicationDbContext _context;
        private UserManager<HatUser> _userManager;


        public CartController(ApplicationDbContext context, UserManager<HatUser> userManager)
        {
            this._userManager = userManager;
            this._context = context;
        }

        public IActionResult Index()
        {
            HatUser hatUser = null;
            if (User.Identity.IsAuthenticated)
            {
                hatUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            }
            Cart cart = CartService.GetCart(_context, Request, Response, hatUser);
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
            HatUser hatUser = null;
            if (User.Identity.IsAuthenticated)
            {
                hatUser = _userManager.FindByNameAsync(User.Identity.Name).Result;
            }
            Cart cart = CartService.GetCart(_context, Request, Response, hatUser);
            return Json(cart.CartItems.Sum(x => x.Quantity));
        }
    }
}