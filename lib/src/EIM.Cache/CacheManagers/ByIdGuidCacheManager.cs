using EIM.Cache.CacheIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Cache.CacheManagers
{
    public class ByIdGuidCacheManager<T> : ByIdCacheManager<T>
        where T : class, IIdGuidProvider
    {
        public ByIdGuidCacheManager()
        {
            
        }

        protected ByGuidCacheIndex<T> DicByGuid { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.DicByGuid = new ByGuidCacheIndex<T>(this);

            List<CacheIndex<T>> cacheIndexes = base.CreateCacheIndexes();
            cacheIndexes.Add(this.DicByGuid);

            return cacheIndexes;
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
