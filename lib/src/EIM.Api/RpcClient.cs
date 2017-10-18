using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace EIM.Api
{

    public class RpcClient
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
        private readonly IBasicProperties props;

        public RpcClient()
        {
            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.ExchangeDeclare("rpc_exchange", "fanout", true, false, null);
            channel.QueueDeclare("rpc_queue", true, false, false, null);
            channel.QueueBind("rpc_queue", "rpc_exchange", "", null);

            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(response);
                }
            };
        }

        public T Reqeust<T>(object args)
        {
            string argsTypeName = args.GetType().FullName;
            props.Headers = new Dictionary<string, object>();
            props.Headers.Add("__argsTypeName__", argsTypeName);

            string message = JsonConvert.SerializeObject(args);
            var messageBytes = Encoding.UTF8.GetBytes(message);
            channel.BasicPublish(
                exchange: "rpc_exchange",
                routingKey: "",
                basicProperties: props,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: "rpc_queue",
                autoAck: true);

            string responseMessage = respQueue.Take();
            return JsonConvert.DeserializeObject<T>(responseMessage);
        }

        public void Close()
        {
            connection.Close();
        }
    }

}
