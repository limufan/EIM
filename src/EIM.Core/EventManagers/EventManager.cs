using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.EventManagers
{
    public class EventManager
    {
        public EventManager()
        {
            this.UserEventManager = new UserEventManager();
        }

        public UserEventManager UserEventManager { set; get; }
    }
}
