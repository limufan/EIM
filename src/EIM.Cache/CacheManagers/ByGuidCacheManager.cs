using EIM.Cache.CacheIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Cache.CacheManagers
{
    public class ByGuidCacheManager<T> : CacheManager<T>
        where T : class, IGuidProvider
    {
        public ByGuidCacheManager()
        {
            
        }

        protected ByGuidCacheIndex<T> DicByGuid { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.DicByGuid = new ByGuidCacheIndex<T>(this);

            return new List<CacheIndex<T>>() { this.DicByGuid };
        }

        public virtual T GetByGuid(string guid)
        {
            return this.DicByGuid.GetByKey(guid);
        }

        public virtual bool ContainsGuid(string guid)
        {
            return this.DicByGuid.ContainsKey(guid);
        }
        

        public int GetByGuidCacheCount()
        {
            return this.DicByGuid.GetCacheCount();
        }
    }
}
