using EIM.Business.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.CacheIndexes
{
    public class ByCodeCacheIndex<T>: DicationaryCacheIndex<T, string>
        where T : class, ICodeProvider
    {
        public ByCodeCacheIndex(CacheManager<T> cacheManager):
            base(cacheManager)
        {
            
        }
        protected override void OnAdded(T cache)
        {
            base.OnAdded(cache);

            if (cache is ICacheCodeChanged<T>)
            {
                (cache as ICacheCodeChanged<T>).CodeChanged += ByGuidCacheManager_CodeChanged;
            }
        }

        private void ByGuidCacheManager_CodeChanged(T sender, CacheCodeChangedArgs args)
        {
            this.Remove(args.SnapshotCode);
            this.Add(sender);
        }

        protected override string GetKey(T cache)
        {
            return cache.Code;
        }
    }
}
