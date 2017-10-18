using EIM.Business.Org;
using EIM.Core.Events;
using EIM.MessageQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.MessageMangers
{
    public class OrgEventMessagePublisher
    {
        public OrgEventMessagePublisher(EventManager eventManager)
        {
            eventManager.UserEvents.Created += UserEvents_Created;
            eventManager.UserEvents.Changed += UserEvents_Changed;
            eventManager.UserEvents.Deleted += UserEvents_Deleted;
        }

        private void UserEvents_Deleted(User sender, Business.OperationInfo args)
        {
            using (UserEventQueue eventQueue = new UserEventQueue())
            {
                eventQueue.SendDeletedMessage(sender.Id);
            }
        }

        private void UserEvents_Changed(User sender, UserChangeInfo args)
        {
            using (UserEventQueue eventQueue = new UserEventQueue())
            {
                eventQueue.SendChangedMessage(sender.Id);
            }
        }

        private void UserEvents_Created(User sender, UserCreateInfo args)
        {
            using (UserEventQueue eventQueue = new UserEventQueue())
            {
                eventQueue.SendCreatedMessage(sender.Id);
            }
        }
    }
}
