using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Threading;

namespace RabbitMQ.Demos.WorkQueues.Worker
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
                    channel.QueueDeclare("task_queue", durable, false, false, null);

                    // don't dispatch a new message to a worker 
                    // until it has processed and acknowledged the previous one.
                    channel.BasicQos(0, 1, false);

                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume("hello", true, consumer);

                    Console.WriteLine(" [*] Waiting for messages.To exit press CTRL+C");

                    while (true)
                    {                        
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

                        var body = ea.Body;
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine(" [x] Received {0}", message);

                        // Simulate Work
                        int dots = message.Split('.').Length - 1;
                        Thread.Sleep(dots * 1000);

                        Console.WriteLine(" [x] Done");

                        // An ack(nowledgement) is sent back from the consumer 
                        // to tell RabbitMQ that a particular message has been received,
                        // processed and that RabbitMQ is free to delete it.
                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
            }
        }
    }
}
