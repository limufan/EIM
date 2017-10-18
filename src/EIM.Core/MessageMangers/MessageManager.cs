using EIM.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.MessageMangers
{
    public class MessageManager
    {
        public MessageManager(EventManager eventManager)
        {
            this.OrgEventMessagePublisher = new OrgEventMessagePublisher(eventManager);
        }

        public OrgEventMessagePublisher OrgEventMessagePublisher { set; get; }
    }
}
