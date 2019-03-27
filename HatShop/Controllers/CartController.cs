﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HatShop.Data;
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
            string cookieData = Request.Cookies.ContainsKey("HatShopCartInfo") ? Request.Cookies["HatShopCartInfo"] : null;
            Cart cart = null;
            Guid cookieIdentifier;
            if (Guid.TryParse(cookieData, out cookieIdentifier))
            {
                cart = _context.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductColors)
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductSizes)
                        .FirstOrDefault(c => c.CookieIdentifier == cookieIdentifier);
            }

            return View(cart);
        }
    }
}