using HatShop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace HatShop.Services
{
    public class CartService
    {
        private const string COOKIE_NAME = "HatShopCartInfo";

        public static Cart GetCart(ApplicationDbContext ctx, HttpRequest req, HttpResponse resp, HatUser user)
        {
            Cart cart = null;

            if(user != null)
            {
                cart = ctx.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductColors)
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductSizes)
                        .FirstOrDefault(c => c.ID == user.CartID);
            }

            //If the user has a previous cart cookie, try to use that cart:
            if (cart == null && req.Cookies.ContainsKey(COOKIE_NAME))
            {
                Guid cookieIdentifier;
                if (Guid.TryParse(req.Cookies[COOKIE_NAME], out cookieIdentifier))
                {
                    cart = ctx.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductColors)
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductSizes)
                        .FirstOrDefault(c => c.CookieIdentifier == cookieIdentifier);

                    if (cart != null && user != null)
                    {
                        cart.HatUser = user;
                        resp.Cookies.Delete("HatShopCartInfo");
                    }

                }
            }

            //If you couldn't use the cart cookie, create a new empty cart
            //and add the cookie ID to the response
            if (cart == null)
            {
                cart = new Cart();
                cart.CookieIdentifier = Guid.NewGuid();
                resp.Cookies.Append(COOKIE_NAME, cart.CookieIdentifier.ToString());
                ctx.Carts.Add(cart);

                if (user != null)
                {
                    cart.HatUser = user;
                    resp.Cookies.Delete("HatShopCartInfo");
                }
            }

            ctx.SaveChanges();

            return cart;
        }
    }
}
