using EIM.Cache.CacheIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Cache.CacheManagers
{
    public class ByCodeCacheManager<T> : CacheManager<T>
        where T : class, ICodeProvider
    {
        public ByCodeCacheManager()
        {

        }

        protected ByCodeCacheIndex<T> ByCodeIndex { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.ByCodeIndex = new ByCodeCacheIndex<T>(this);

            return new List<CacheIndex<T>>() { this.ByCodeIndex };
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
