using EIM.Business.CacheIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business.CacheManagers
{
    public class ByIdCodeCacheManager<T> : CacheManager<T>
        where T : class, IIdCodeProvider
    {
        protected ByIdCacheIndex<T> ByIdIndex { private set; get; }

        protected ByCodeCacheIndex<T> ByCodeIndex { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.ByIdIndex = new ByIdCacheIndex<T>(this);
            this.ByCodeIndex = new ByCodeCacheIndex<T>(this);

            return new List<CacheIndex<T>>() { this.ByIdIndex, this.ByCodeIndex };
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
