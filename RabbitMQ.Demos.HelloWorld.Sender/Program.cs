using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQ.Demos.HelloWorld.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1. create a connection to a broker on the local machine
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                // 2. In the channel is where the api resides
                using (var channel = connection.CreateModel())
                {
                    // 3. we must declare a queue for us to send to
                    // Declaring a queue is idempotent:
                    // it will only be created if it doesn't exist already.
                    channel.QueueDeclare("hello", false, false, false, null);

                    // the message content is a byte array,
                    string message = "Hello World!";
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", "hello", null, body);

                    Console.WriteLine(" [x] Sent {0}", message);
                }
            }
        }
    }
}
