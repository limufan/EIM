using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Newtonsoft.Json;
using System.Reflection;
using EIM.Api.Org;

namespace EIM.Services
{
    public class RPCServer
    {
        public Dictionary<Type, MethodInfo> _serviceMethods;

        public RPCServer()
        {
            this._serviceMethods = new Dictionary<Type, MethodInfo>();
            this._serviceMethods.Add(typeof(UserCreateModel), typeof(UserService).GetMethod("Create"));

            var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("rpc_exchange", "fanout", true, false, null);
                channel.QueueDeclare("rpc_queue", true, false, false, null);
                channel.QueueBind("rpc_queue", "rpc_exchange", "", null);
                channel.BasicQos(0, 1, false);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue: "rpc_queue",
                  autoAck: false, consumer: consumer);
                Console.WriteLine(" [x] Awaiting RPC requests");

                consumer.Received += (model, ea) =>
                {
                    string response = null;

                    var body = ea.Body;
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var message = Encoding.UTF8.GetString(body);
                        string argsTypeName = props.Headers["__argsTypeName__"].ToString();
                        Type argsType = Type.GetType(argsTypeName);
                        object args = JsonConvert.DeserializeObject(message, argsType);
                        
                        response = this.Call(args);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);
                        channel.BasicPublish(exchange: "rpc_exchange", routingKey: props.ReplyTo,
                          basicProperties: replyProps, body: responseBytes);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag,
                          multiple: false);
                    }
                };

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
        
        private string Call(object args)
        {
            MethodInfo method = this._serviceMethods[args.GetType()];
            object instance = Activator.CreateInstance(method.ReflectedType);

            object obj = method.Invoke(instance, new object[] { args });

            return JsonConvert.SerializeObject(obj);
        }
    }

}
