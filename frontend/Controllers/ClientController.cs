using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace front.Controllers
{
    public class ClientController : Controller
    {
        [HttpGet]
        [Route("clients")]
        public IActionResult ClientPage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ClientPagePost([FromForm] string id)
        {
            var httpClient = HttpClientFactory.Create();

            var url = $"http://sender/send/get/{id}";

            Console.WriteLine(url);
            
            var data = await httpClient.GetStringAsync(url);

            ViewData["clientFound"] = data.Equals("") ? "0" : "1";
            ViewData["clientID"] = id;
            ViewData["clientAmount"] = data;
            
            return View();
        }

        [HttpGet]
        [Route("wykonaj-przelew")]
        public IActionResult WykonajPrzelewPage()
        {
            return View();
        }
        
        [HttpPost]
        [Route("wykonaj-przelew")]
        public async Task<IActionResult> WykonajPrzelewPagePost([FromForm] string from, [FromForm] string to, [FromForm] string amount)
        {
            var httpClient = HttpClientFactory.Create();
            
            var url = $"http://sender/przelew/{from}/{to}/{amount}";

            try
            {
                var data = await httpClient.GetStringAsync(url);
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            
            return View();
        }
    }
}
