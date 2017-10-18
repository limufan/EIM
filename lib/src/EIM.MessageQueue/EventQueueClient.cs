using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.MessageQueue
{
    public class EventQueueClient: IDisposable
    {

        ConnectionFactory _connectionFactory;
        IConnection _connection;
        IModel _channel;
        string _exchangeName;
        string _queueName;

        public EventQueueClient()
        {
            this._connectionFactory = new ConnectionFactory() { HostName = MessageQueueConfig.RabbitHost, UserName = MessageQueueConfig.RabbitUserName, Password = MessageQueueConfig.RabbitPassword };
            this._connection = this._connectionFactory.CreateConnection();
            this._channel = this._connection.CreateModel();
            this._exchangeName = "eim_event_exchange";
            this._queueName = "event_queue";
            this._channel.ExchangeDeclare(this._exchangeName, "fanout", true, false, null);
        }

        public void SendMessage(string eventType, object args)
        {
            string message = JsonConvert.SerializeObject(args);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = this._channel.CreateBasicProperties();
            properties.Persistent = true;

            this._channel.BasicPublish(this._exchangeName, eventType, properties, body);
        }

        public void BindMessage<T>(string eventType, Action<T> action)
        {
            this._channel.QueueDeclare(this._queueName, true, false, false, null);
            this._channel.QueueBind(this._queueName, this._exchangeName, eventType, null);

            var consumer = new EventingBasicConsumer(this._channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                T args = JsonConvert.DeserializeObject<T>(message);

                action(args);

                this._channel.BasicAck(ea.DeliveryTag, false);
            };
            this._channel.BasicConsume(this._queueName, false, consumer);
        }

        public void Dispose()
        {
            this._connection.Dispose();
            this._channel.Dispose();
        }
    }
}
