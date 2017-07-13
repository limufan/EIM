using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM.Core
{
    public class CheckCacheResult<CacheType>
    {
        public List<CacheType> RemoveCacheList { set; get; }

        public List<CacheType> AddOrUpdateCache { set; get; }
    }
}
