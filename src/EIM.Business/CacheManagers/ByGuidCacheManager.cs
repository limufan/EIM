using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business
{
    public class ByGuidCacheManager<T> : CacheManager<T>
        where T : class, IGuidProvider
    {
        public ByGuidCacheManager()
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
                (cache as ICacheGuidChanged<T>).GuidChanged += ByGuidCacheManager_GuidChanged;
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
                EIMLog.Logger.Warn(string.Format("{0} Guid 重复Guid: {1} Guid: {2}", this.GetType().Name, cache.Guid, cache.Guid));
            }
        }

        void ByGuidCacheManager_GuidChanged(T sender, CacheGuidChangedArgs args)
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

            return null;
        }

        public int GetByGuidCacheCount()
        {
            return this.DicByGuid.Count;
        }
    }
}
