using FireOnWheels.Messaging;
using FireOnWheels.Registration.Web.Messages;
using FireOnWheels.Registration.Web.ViewModels;
using Microsoft.AspNet.Mvc;

namespace FireOnWheels.Registration.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RegisterOrder() {
            return View();
        }

        [HttpPost]
        public string RegisterOrder(OrderViewModel model) {
            var registerOrderCommand = new RegisterOrderCommand(model);
            using(var rabbitCtlr = new RabbitMqManager()) {
                rabbitCtlr.SendRegisterOrderCommand(registerOrderCommand);
            }
            return "Thanks!";
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
