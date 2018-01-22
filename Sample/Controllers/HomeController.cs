using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WhatsApp;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWhatsAppConnector whatsapp;

        public HomeController(IWhatsAppConnector whatsapp)
        {
            this.whatsapp = whatsapp;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("~/whatsapp/ui")]
        public IActionResult Screenshot()
        {
            var contents = System.IO.File.ReadAllBytes($@"c:\whatsapp\screen.jpg");

            return File(contents,"image/jpg");
        }
       
        [HttpPost("~/whatsapp/send")]
        public async Task<IActionResult> Send(string nr, string message)
        {
            await whatsapp.SendMessageAsync(nr, message); //nr is international format without the '+', eg. 31612345678

            return Ok();
        }

    
    }
}
