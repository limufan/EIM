using EIM.Cache.CacheIndexes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Cache.CacheManagers
{
    public class ByIdCacheManager<T> : CacheManager<T>
        where T : class, IIdProvider
    {
        public ByIdCacheManager()
        {
            
        }
        protected ByIdCacheIndex<T> ByIdIndex { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.ByIdIndex = new ByIdCacheIndex<T>(this);

            return new List<CacheIndex<T>>() { this.ByIdIndex };
        }

        public virtual T GetById(int id)
        {
            return this.ByIdIndex.GetByKey(id);
        }

        public virtual bool ContainsId(int id)
        {
            return this.ByIdIndex.ContainsKey(id);
        }

        public int GetByIdCacheCount()
        {
            return this.ByIdIndex.GetCacheCount();
        }
    }
}
