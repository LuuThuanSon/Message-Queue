using RabbitMQ.Client;
using System;

namespace MessageQueue.Producer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.VirtualHost = "/";
            factory.HostName = "localhost";
            factory.Port = 5672;
            //factory.ClientProvidedName = "app:audit component:event-consumer"
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    //channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
                    //channel.QueueDeclare(queueName, false, false, false, null);
                    //channel.QueueBind(queueName, exchangeName, routingKey, null);
                    //channel.QueueDeclareNoWait(queueName, true, false, false, null);
                    //channel.QueueDelete("queue-name", false, false);
                    channel.QueueDeclare(queue: "hello",
                                         durable: true,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                    byte[] body = System.Text.Encoding.UTF8.GetBytes("Hello, world!");
                    IBasicProperties props = channel.CreateBasicProperties();
                    props.ContentType = "text/plain";
                    props.DeliveryMode = 2;
                    props.Expiration = "36000000";
                    //props.Headers = new Dictionary<string, object>();
                    //props.Headers.Add("latitude", 51.5252949);
                    //props.Headers.Add("longitude", -0.0905493);
                    channel.BasicPublish(exchange: "",
                                         routingKey: "hello",
                                         basicProperties: props,
                                         body: body);
                    Console.WriteLine("Send Hello World !");
                    Console.ReadLine();
                    //var response = channel.QueueDeclarePassive("queue-name");
                    //// returns the number of messages in Ready state in the queue
                    //response.MessageCount;
                    //// returns the number of consumers the queue has
                    //response.ConsumerCount;
                }
            }
        }
    }
}