using EIM.Cache.CacheIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Cache.CacheManagers
{
    public class ByGuidCodeCacheManager<T> : CacheManager<T>
        where T : class, IGuidCodeProvider
    {

        protected ByCodeCacheIndex<T> ByCodeIndex { private set; get; }
        protected ByGuidCacheIndex<T> DicByGuid { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.ByCodeIndex = new ByCodeCacheIndex<T>(this);
            this.DicByGuid = new ByGuidCacheIndex<T>(this);

            return new List<CacheIndex<T>>() { this.ByCodeIndex, this.DicByGuid };
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

        public virtual T GetByCode(string code)
        {
            return this.ByCodeIndex.GetByKey(code);
        }

        public virtual bool ContainsCode(string code)
        {
            return this.ByCodeIndex.ContainsKey(code);
        }

        public int GetByCodeCacheCount()
        {
            return this.ByCodeIndex.GetCacheCount();
        }
    }
}
