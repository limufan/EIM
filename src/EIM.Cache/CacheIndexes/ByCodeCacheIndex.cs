using EIM.Cache.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Cache.CacheIndexes
{
    public class ByCodeCacheIndex<T>: DicationaryCacheIndex<T, string>
        where T : class, ICodeProvider
    {
        public ByCodeCacheIndex(CacheManager<T> cacheManager):
            base(cacheManager)
        {
            
        }

        protected override string GetKey(T cache)
        {
            return cache.Code;
        }
    }
}
