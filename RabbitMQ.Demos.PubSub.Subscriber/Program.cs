using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace RabbitMQ.Demos.PubSub.Subscriber
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
                    channel.ExchangeDeclare("logs", "fanout");

                    // Temporary queues:
                    // whenever we connect to Rabbit we want an empty queue
                    // once we disconnect the queue should be automatically deleted
                    var queueName = channel.QueueDeclare().QueueName;

                    // Binding:
                    // tell the exchange to send messages to our queue
                    channel.QueueBind(queueName, "logs", "");

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(queueName, true, consumer);

                    Console.WriteLine(" [*] Waiting for messages.To exit press CTRL+C");

                    while (true)
                    {
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
