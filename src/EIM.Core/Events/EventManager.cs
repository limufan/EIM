using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.Events
{
    public class EventManager
    {
        public EventManager()
        {
            this.UserEvents = new UserEvents();
        }

        public UserEvents UserEvents { set; get; }
    }
}
