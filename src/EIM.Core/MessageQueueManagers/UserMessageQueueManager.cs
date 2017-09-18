using EIM.Business.Org;
using EIM.Core.EventManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.MessageQueueManagers
{
    public class UserMessageQueueManager
    {
        public UserMessageQueueManager(UserEventManager userEventManager)
        {
            userEventManager.Created += UserEventManager_Created;
        }

        private void UserEventManager_Created(User sender, UserCreateInfo args)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(User sender)
        {

        }
    }
}
