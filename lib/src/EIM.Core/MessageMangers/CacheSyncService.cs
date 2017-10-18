using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.MessageMangers
{
    public class CacheSyncService
    {
        public CacheSyncService(EIMCacheProviderManager cacheProviderManager)
        {
            this.OrgCacheSynchronizer = new OrgCacheSynchronizer(cacheProviderManager);
        }

        public OrgCacheSynchronizer OrgCacheSynchronizer { set; get; }

        public void Start()
        {
            this.OrgCacheSynchronizer.Sync();
        }
    }
}
