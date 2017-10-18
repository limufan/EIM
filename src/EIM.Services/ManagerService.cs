using EIM.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM.Services
{
    public class ManagerService : EIMService
    {
        public void Any(__ReloadDTO__ dto)
        {
            
        }

        public void Any(__DisableServerDTO__ dto)
        {
            
        }

        public void Any(__EnableServerDTO__ dto)
        {
            
        }

        public ServerStatus Any(__GetServerStatusDTO__ dto)
        {
            return ServerStatus.Enable;
        }
    }
}
