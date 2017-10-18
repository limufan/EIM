using EIM.MessageQueue;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.MessageQueue
{
    public class UserEventQueue : IDisposable
    {
        EventQueueClient _mqClient;

        public UserEventQueue()
        {
            this._mqClient = new EventQueueClient();

        }

        public void BindCreatedMessage(Action<int> action)
        {
            this._mqClient.BindMessage<int>("user_created", action);
        }

        public void BindChangedMessage(Action<int> action)
        {
            this._mqClient.BindMessage<int>("user_changed", action);
        }

        public void BindDeletedMessage(Action<int> action)
        {
            this._mqClient.BindMessage<int>("user_deleted", action);
        }

        public void SendCreatedMessage(int userId)
        {
            this._mqClient.SendMessage("user_created", userId);
        }

        public void SendChangedMessage(int userId)
        {
            this._mqClient.SendMessage("user_changed", userId);
        }

        public void SendDeletedMessage(int userId)
        {
            this._mqClient.SendMessage("user_deleted", userId);
        }

        public void Dispose()
        {
            this._mqClient.Dispose();
        }
    }
}
