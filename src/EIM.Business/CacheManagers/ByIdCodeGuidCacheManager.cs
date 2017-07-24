using EIM.Business.CacheIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business.CacheManagers
{
    public class ByIdCodeGuidCacheManager<T> : ByIdCodeCacheManager<T>
        where T : class, IIdCodeGuidProvider
    {
        public ByIdCodeGuidCacheManager()
        {
            
        }

        protected ByGuidCacheIndex<T> ByGuidCacheIndex { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.ByGuidCacheIndex = new ByGuidCacheIndex<T>(this);

            List<CacheIndex<T>> cacheIndexes = base.CreateCacheIndexes();
            cacheIndexes.Add(this.ByGuidCacheIndex);

            return cacheIndexes;
        }

        public T GetByGuid(string name)
        {
            return this.ByGuidCacheIndex.GetByKey(name);
        }
    }
}
