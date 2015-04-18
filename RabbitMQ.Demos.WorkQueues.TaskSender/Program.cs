using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ.Demos.WorkQueues.TaskSender
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
                    // we need to mark both the queue and messages as durable
                    bool durable = true;
                    channel.QueueDeclare("hello", durable, false, false, null);

                    // Allow arbitrary messages to be sent from the command line
                    string message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);

                    // Persistent 
                    var properties = channel.CreateBasicProperties();
                    properties.SetPersistent(true);

                    channel.BasicPublish("", "hello", properties, body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return (args.Length > 0) ? string.Join("", args) : "Hello World!";
        }
    }
}
