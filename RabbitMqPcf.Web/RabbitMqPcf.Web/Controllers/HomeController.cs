using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMqPcf.Web.Models;

namespace RabbitMqPcf.Web.Controllers
{
    public class HomeController : Controller
    {
        
        private static readonly string DefaultHostNameKey = "rabbitmq:default:host";
        private static readonly string DefaultQueueNameKey = "rabbitmq:default:queue";

        private IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View(new SendMessageModel
            {
                HostName = _configuration[DefaultHostNameKey],
                QueueName = _configuration[DefaultQueueNameKey],
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]        
        public IActionResult Index(SendMessageModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            
            var factory = new ConnectionFactory() { HostName = model.HostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: model.QueueName, 
                        durable: false, 
                        exclusive: false, 
                        autoDelete: false, 
                        arguments: null);
                                        
                    var body = Encoding.UTF8.GetBytes(model.MessageBody);
                    channel.BasicPublish(
                        exchange: "",
                        routingKey: model.QueueName,
                        basicProperties: null,
                        body: body);
                }
            }

            return RedirectToAction(nameof(Success));            
        }

        public IActionResult Success()
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
