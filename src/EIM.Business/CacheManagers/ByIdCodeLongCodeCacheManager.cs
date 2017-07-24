using EIM.Business.CacheIndexes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business.CacheManagers
{
    public class ByIdCodeLongCodeCacheManager<T> : ByIdCodeCacheManager<T>
        where T : class, IIdCodeLongCodeProvider
    {
        public ByIdCodeLongCodeCacheManager()
        {
            
        }

        protected ByLongCodeCacheIndex<T> ByLongCodeCacheIndex { private set; get; }

        protected override List<CacheIndex<T>> CreateCacheIndexes()
        {
            this.ByLongCodeCacheIndex = new ByLongCodeCacheIndex<T>(this);

            List<CacheIndex<T>> cacheIndexes = base.CreateCacheIndexes();
            cacheIndexes.Add(this.ByLongCodeCacheIndex);

            return cacheIndexes;
        }

        public T GetByLongCode(string longCode)
        {
            return this.ByLongCodeCacheIndex.GetByKey(longCode);
        }        
    }
}
