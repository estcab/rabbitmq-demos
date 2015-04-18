using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Demos.HelloWorld.Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            // open a connection and a channel, 
            // and declare the queue from which we're going to consume
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    // same as the sender queue
                    channel.QueueDeclare("hello", false, false, false, null);

                    // callback in the form of an object that will 
                    // buffer the messages until we're ready to use them
                    var consumer = new QueueingBasicConsumer(channel);                    
                    
                    channel.BasicConsume("hello", true, consumer);

                    Console.WriteLine(" [*] Waiting for messages.To exit press CTRL+C");

                    while (true)
                    {
                        //blocks until another message has been delivered from the server 
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine(" [x] Received {0}", message);
                    }
                }
            }
        }
    }
}
