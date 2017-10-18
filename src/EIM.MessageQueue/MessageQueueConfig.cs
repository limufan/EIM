using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.MessageQueue
{
    public class MessageQueueConfig
    {
        static MessageQueueConfig()
        {
            RabbitHost = "localhost";
            RabbitUserName = "guest";
            RabbitPassword = "guest";
        }

        public static string RabbitHost { get; }

        public static string RabbitUserName { get; }

        public static string RabbitPassword { get; }
    }
}
