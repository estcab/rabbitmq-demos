using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.Demos.Routing.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // direct exchange
                    // a message goes to the queues whose binding key 
                    // exactly matches the routing key of the message
                    channel.ExchangeDeclare("direct_logs", "direct");

                    // binding key
                    var severity = (args.Length > 0) ? args[0] : "info";

                    var message = (args.Length > 1)
                                ? string.Join(" ", args.Skip(1).ToArray())
                                : "Hello World!";

                    var body = Encoding.UTF8.GetBytes(message);
                    channel.BasicPublish("direct_logs", severity, null, body);
                    
                    Console.WriteLine(" [x] Sent '{0}':'{1}'", severity, message);
                }
            }
        }
    }
}
