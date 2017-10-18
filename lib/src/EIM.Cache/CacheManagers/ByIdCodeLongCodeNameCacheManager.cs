using EIM.Cache.CacheIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Cache.CacheManagers
{
    public class ByIdCodeLongCodeNameCacheManager<T> : ByIdCodeLongCodeCacheManager<T>
        where T : class, IIdCodeLongCodeNameProvider
    {
        public ByIdCodeLongCodeNameCacheManager()
        {
            
        }

        protected ByNameCacheIndex<T> ByNameCacheIndex { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.ByNameCacheIndex = new ByNameCacheIndex<T>(this);

            List<CacheIndex<T>> cacheIndexes = base.CreateCacheIndexes();
            cacheIndexes.Add(this.ByNameCacheIndex);

            return cacheIndexes;
        }

        public T GetByName(string name)
        {
            return this.ByNameCacheIndex.GetByKey(name);
        }
    }
}
