using EIM.Cache.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Cache.CacheIndexes
{
    public class ByIdCacheIndex<T>: DicationaryCacheIndex<T, int>
        where T : class, IIdProvider
    {
        public ByIdCacheIndex(CacheManager<T> cacheManager):
            base(cacheManager)
        {

        }

        protected override int GetKey(T cache)
        {
            return cache.Id;
        }
    }
}
