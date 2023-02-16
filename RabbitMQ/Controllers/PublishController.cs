using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RabbitMQ.Controllers
{
    [Route("api/[controller]")]
    public class PublishController : ControllerBase
    {
        private readonly ConnectionFactory _connectionFactory;
        private const string QUEUE_NAME = "messages";

        public PublishController()
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] PublishInputModel message)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: QUEUE_NAME,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    var stringfieldMessage = JsonConvert.SerializeObject(message);
                    var bytesMessage = Encoding.UTF8.GetBytes(stringfieldMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: QUEUE_NAME,
                        basicProperties: null,
                        body: bytesMessage
                        );
                }
            }

            return Accepted();
        }
    }
}

