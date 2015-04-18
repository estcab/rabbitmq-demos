using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Demos.PubSub.Emit
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
                    // the producer can only send messages to an exchange
                    // receives messages from producers and pushes them to queues
                    // types: direct, topic, headers and fanout.

                    channel.ExchangeDeclare("logs", "fanout");

                    var message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("logs", "", null, body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0) ? string.Join(" ", args) : "info: Hello World!");
        }
    }
}
