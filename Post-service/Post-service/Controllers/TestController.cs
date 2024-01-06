using Microsoft.AspNetCore.Mvc;
using RabbitMQ_Messenger_Lib;
using RabbitMQ_Messenger_Lib.Types;
using System.Reflection;

namespace Post_service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private Sender sender;

        public TestController(MessengerConfig messengerConfig)
        {
            sender = new Sender(messengerConfig, new List<Queue> { new Queue(name: "LOG") });
        }

        [HttpPost]
        [Route("SendLog")]
        public IActionResult SendLog(string message, int priority)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("message", message);
            dict.Add("priority", priority);

            Message _message = new Message(dict, MessageType.LOG, $"{Assembly.GetExecutingAssembly().GetName().Name}");
            sender.Send(_message, "LOG");
            return Ok();
        }
    }
}