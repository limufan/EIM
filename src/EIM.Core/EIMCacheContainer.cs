using EIM.Cache.CacheManagers;
using EIM.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIM.Business.Org;

namespace EIM.Core
{
    public class EIMCacheContainer: CacheContainer
    {
        public EIMCacheContainer()
        {
            this.UserCacheManager = this.CreateManager<UserCacheManager>();
        }

        public UserCacheManager UserCacheManager { set; get; }
    }
}
