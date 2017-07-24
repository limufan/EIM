using EIM.Business.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.CacheIndexes
{
    public class KeyFuncCacheIndex<CacheType, KeyType>: DicationaryCacheIndex<CacheType, KeyType>
        where CacheType : class
    {
        public KeyFuncCacheIndex(CacheManager<CacheType> cacheManager, Func<CacheType, KeyType> keyFunc) :
            base(cacheManager)
        {
            this.KeyFunc = keyFunc;
        }

        public Func<CacheType, KeyType> KeyFunc { private set; get; }

        protected override KeyType GetKey(CacheType cache)
        {
            return this.KeyFunc(cache);
        }
    }
}
