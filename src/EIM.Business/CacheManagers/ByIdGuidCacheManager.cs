using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business
{
    public class ByIdGuidCacheManager<T> : ByIdCacheManager<T>
        where T : class, IIdGuidProvider
    {
        public ByIdGuidCacheManager()
        {
            this.DicByGuid = new Dictionary<string, T>();
        }

        protected Dictionary<string, T> DicByGuid { private set; get; }

        protected override void _Add(T cache)
        {
            base._Add(cache);
            this.AddCacheToDic(cache);
            if(cache is ICacheGuidChanged<T>)
            {
                (cache as ICacheGuidChanged<T>).GuidChanged += ByIdGuidCacheManager_GuidChanged;
            }
        }

        private void AddCacheToDic(T cache)
        {
            if (!this.DicByGuid.ContainsKey(cache.Guid))
            {
                this.DicByGuid.Add(cache.Guid, cache);
            }
            else
            {
                EIMLog.Logger.Info(string.Format("{0} Guid 重复ID: {1} Guid: {2}", this.GetType().Name, cache.ID, cache.Guid));
            }
        }

        void ByIdGuidCacheManager_GuidChanged(T sender, CacheGuidChangedArgs args)
        {
            this.DicByGuid.Remove(args.SnapshotGuid);
            this.AddCacheToDic(sender);
        }

        protected override void _Remove(T cache)
        {
            base._Remove(cache);
            this.DicByGuid.Remove(cache.Guid);
        }

        public virtual T GetByGuid(string guid)
        {
            this.EnableValidate();
            if (string.IsNullOrEmpty(guid))
            {
                return default(T);
            }

            this.Lock.AcquireReaderLock(10000);
            try
            {
                if (this.DicByGuid.ContainsKey(guid))
                {
                    return this.DicByGuid[guid];
                }
                return default(T);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
            }
        }

        public virtual bool ContainsGuid(string guid)
        {
            this.EnableValidate();
            if (string.IsNullOrEmpty(guid))
            {
                return false;
            }

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return this.DicByGuid.ContainsKey(guid);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
            }
        }

        protected override void _Clear()
        {
            base._Clear();
            this.DicByGuid.Clear();
        }

        public override object Get(object key)
        {
            if (key == null)
            {
                return null;
            }

            string guid = key.ToString();

            if (guid.Length == 36)
            {
                return this.GetByGuid(guid);
            }

            return base.Get(key);
        }

        public int GetByGuidCacheCount()
        {
            return this.DicByGuid.Count;
        }
    }
}
