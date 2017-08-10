using EIM.Cache.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Cache.CacheIndexes
{
    public abstract class CacheIndex<T> where T : class
    {
        public CacheIndex(CacheManager<T> cacheManager)
        {
            this.CacheManager = cacheManager;
            this.CacheManager.Added += CacheManager_Added;
            this.CacheManager.Removed += CacheManager_Removed;
            this.CacheManager.Cleared += CacheManager_Cleared;
            this.CacheManager.Changed += CacheManager_Changed;
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

        private void CacheManager_Changed(CacheChangedArgs<T> args)
        {
            this.Change(args);
        }

        public abstract void Add(T cache);

        public abstract void Remove(T cache);

        public abstract void Clear();

        public abstract void Change(CacheChangedArgs<T> args);

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
