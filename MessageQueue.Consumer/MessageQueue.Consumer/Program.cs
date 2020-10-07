using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessageQueue.Consumer
{
    public class CustomTaskScheduler : TaskScheduler
    {
        // ...
        protected override IEnumerable<Task> GetScheduledTasks()
        {
            throw new NotImplementedException();
        }

        protected override void QueueTask(Task task)
        {
            throw new NotImplementedException();
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }
    }

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
            //factory.DispatchConsumersAsync = true;
            //factory.AutomaticRecoveryEnabled = true;
            //factory.TaskScheduler = new CustomTaskScheduler();
            //factory.ClientProvidedName = "app:audit component:event-consumer"
            using (var connection = factory.CreateConnection())
            {

                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);


                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += Consumer_Received;
                    channel.BasicConsume(queue: "hello",
                    autoAck: true,
                    consumer: consumer);

                    //string tag = channel.BasicConsume("hello", false, consumer);
                    //// this consumer tag identifies the subscription
                    //// when it has to be cancelled


                    //bool noAck = false;

                    //BasicGetResult result = channel.BasicGet("hello", noAck);
                    //if (result == null)
                    //{
                    //    // No message available at this time.
                    //}
                    //else
                    //{
                    //    IBasicProperties props = result.BasicProperties;
                    //    ReadOnlyMemory<byte> body = result.Body;
                    //    channel.BasicAck(result.DeliveryTag, false);
                    //}
                    // ensure we get a delivery
                    //bool waitRes = latch.WaitOne(2000);
                }
            }
        }
        private static void Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine(" [x] Received {0}", message);
            Console.ReadLine();
        }
    }
}