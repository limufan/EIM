using EIM.Cache.CacheManagers;
using EIM.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIM.Business.Org;
using EIM.Core.Events;

namespace EIM.Core
{
    public class EIMCacheContainer: CacheContainer
    {
        public EIMCacheContainer(EventManager eventManager)
        {
            this.UserCacheManager = this.CreateManager<UserCacheManager>(eventManager);
        }

        public UserCacheManager UserCacheManager { set; get; }
    }
}
