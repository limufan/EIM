using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.CacheManagers
{
    public abstract class CacheIndex<T> where T : class
    {
        public CacheIndex(CacheManager<T> cacheManager)
        {
            this.CacheManager = cacheManager;
            this.CacheManager.Added += CacheManager_Added;
            this.CacheManager.Removed += CacheManager_Removed;
            this.CacheManager.Cleared += CacheManager_Cleared;
        }

        public CacheManager<T> CacheManager { set; get; }

        private void CacheManager_Added(T cache)
        {
            this.Add(cache);
        }

        private void CacheManager_Removed(T cache)
        {
            this.Remove(cache);
        }

        private void CacheManager_Cleared()
        {
            this.Clear();
        }

        protected abstract void Add(T cache);

        protected abstract void Remove(T cache);

        protected abstract void Clear();

        public abstract object Get(object key);

        public void AcquireReaderLock()
        {
            this.CacheManager.AcquireReaderLock();
        }

        public void ReleaseReaderLock()
        {
            this.CacheManager.ReleaseReaderLock();
        }

        public void EnableValidate()
        {
            this.CacheManager.EnableValidate();
        }
    }
}
