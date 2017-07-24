using EIM.Business.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.CacheIndexes
{
    public class ByLongCodeCacheIndex<T>: DicationaryCacheIndex<T, string>
        where T : class, ILongCodeProvider
    {
        public ByLongCodeCacheIndex(CacheManager<T> cacheManager):
            base(cacheManager)
        {
            
        }

        protected override string GetKey(T cache)
        {
            return cache.LongCode;
        }
    }
}
