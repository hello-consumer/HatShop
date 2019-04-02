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
        private Braintree.IBraintreeGateway _braintreeGateway;

        public CheckoutController(ApplicationDbContext context, UserManager<HatUser> userManager
        , IEmailSender emailSender, Braintree.IBraintreeGateway braintreeGateway)
        {
            this._context = context;
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._braintreeGateway = braintreeGateway;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["clientToken"] = await _braintreeGateway.ClientToken.GenerateAsync();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutViewModel model, string braintreeNonce)
        {
            ViewData["clientToken"] = await _braintreeGateway.ClientToken.GenerateAsync();
            if (string.IsNullOrEmpty(braintreeNonce))
            {
                this.ModelState.AddModelError("nonce", "We're unable to validate this credit card");

            }

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
                    var orderId = Guid.NewGuid().ToString().Substring(0, 8);
                    Braintree.TransactionRequest transactionRequest = new Braintree.TransactionRequest();
                    transactionRequest.PaymentMethodNonce = braintreeNonce;
                    transactionRequest.PurchaseOrderNumber = orderId;
                    transactionRequest.Amount = cart.CartItems.Sum(x => x.Quantity * x.Product.Price);
                    transactionRequest.ShippingAddress = new Braintree.AddressRequest
                    {
                        StreetAddress = model.ShippingStreet1,
                        ExtendedAddress = model.ShippingStreet2,
                        PostalCode = model.ShippingPostalCode,
                        //CountryName = model.ShippingCountry,  //This thing is picky about casing
                        FirstName = model.ContactName.Split(' ').First(),
                        LastName = model.ContactName.Contains(' ') ? string.Join(' ', model.ContactName.Split(' ').Skip(1)) : "",
                        Locality = model.ShippingCity,
                        Region = model.ShippingRegion
                    };
                    transactionRequest.Customer = new Braintree.CustomerRequest
                    {
                        Email = hatUser != null ? hatUser.Email : model.ContactEmail, 
                    };
                    transactionRequest.LineItems = cart.CartItems.Select(x => new Braintree.TransactionLineItemRequest
                    {
                        Name = x.Product.Name,
                        Description = x.Product.Description,
                        ProductCode = x.ProductID.ToString(),
                        Quantity = x.Quantity,
                        UnitAmount = x.Product.Price,
                        TotalAmount = x.Product.Price * x.Quantity,
                        LineItemKind = Braintree.TransactionLineItemKind.DEBIT
                    }).ToArray();

                    Braintree.Result<Braintree.Transaction> transactionResult = _braintreeGateway.Transaction.Sale(transactionRequest);
                    if (transactionResult.IsSuccess())
                    {
                        //TODO: Get a lot more info here, validate credit card + address, save it to a database
                        Order order = new Order();
                        order.ID = orderId;
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
                }
                ModelState.AddModelError("cart", "There was a problem processing your cart");


            }
            return View(model);
        }
    }
}