using HatShop.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HatShop.Services
{
    public class CartService
    {
        public static Cart GetCart(Microsoft.AspNetCore.Http.HttpRequest req, ApplicationDbContext ctx, Microsoft.AspNetCore.Http.HttpResponse resp)
        {
            Cart cart = null;
            //If the user has a previous cart cookie, try to use that cart:
            if (req.Cookies.ContainsKey("HatShopCartInfo"))
            {
                Guid cookieIdentifier;
                if (Guid.TryParse(req.Cookies["HatShopCartInfo"], out cookieIdentifier))
                {
                    cart = ctx.Carts
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductColors)
                        .Include(c => c.CartItems)
                        .ThenInclude(ci => ci.Product)
                        .ThenInclude(p => p.ProductSizes)
                        .FirstOrDefault(c => c.CookieIdentifier == cookieIdentifier);
                }
            }

            //If you couldn't use the cart cookie, create a new empty cart
            //and add the cookie ID to the response
            if (cart == null)
            {
                cart = new Cart();
                cart.CookieIdentifier = Guid.NewGuid();
                resp.Cookies.Append("HatShopCartInfo", cart.CookieIdentifier.ToString());
                ctx.Carts.Add(cart);
            }

            return cart;
        }
    }
}
