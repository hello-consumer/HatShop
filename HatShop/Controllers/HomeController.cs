using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HatShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HatShop.Controllers
{
    public class HomeController : Controller
    {
        private string _myApiKey;
        private ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _myApiKey = configuration.GetValue<string>("myApiKey");
            _logger = logger;
        }

        public IActionResult Bored()
        {

            string endpoint = "https://www.boredapi.com/api/activity?myApiKey=" + _myApiKey;
            try
            {
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                string response = httpClient.GetStringAsync(endpoint).Result;
                BoredViewModel typedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<BoredViewModel>(response);
                return View(typedResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Content("Can't get this right now");
            }
        }

        public async Task<IActionResult> Stocks()
        {
            string endpoint = "https://api.iextrading.com/1.0/ref-data/symbols";
            try
            {
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                string response = await httpClient.GetStringAsync(endpoint);
                var typedResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Newtonsoft.Json.Linq.JObject>>(response);
                var stocks = typedResponse.Select(stock => new Stock
                {
                    Symbol = stock.GetValue("symbol").ToString(),
                    Name = stock.GetValue("name").ToString()
                }).ToArray();
                return Json(stocks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Content("Can't get this right now");
            }

        }

        private class Stock
        {
            public string Symbol { get; set; }
            public string Name { get; set; }
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
