using EIM.Cache.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Cache.CacheIndexes
{
    public abstract class DicationaryCacheIndex<CacheType, KeyType>: CacheIndex<CacheType>
        where CacheType : class
    {
        public DicationaryCacheIndex(CacheManager<CacheType> cacheManager) :
            base(cacheManager)
        {
            this.DicByKey = new Dictionary<KeyType, CacheType>();
        }

        protected Dictionary<KeyType, CacheType> DicByKey { private set; get; }

        protected abstract KeyType GetKey(CacheType cache);

        public virtual void Remove(KeyType key)
        {
            CacheType cache = this.GetByKey(key);
            if (cache != null)
            {
                this.Remove(cache);
            }
        }

        public override void Add(CacheType cache)
        {
            KeyType key = this.GetKey(cache);
            if (this.DicByKey.ContainsKey(key))
            {
                EIMLog.Logger.WarnFormat("{0} 重复key: {1}", this.GetType().Name, key);
                return;
            }
            this.DicByKey.Add(key, cache);
        }

        protected virtual void OnAdded(CacheType cache)
        {

        }

        public override void Remove(CacheType cache)
        {
            KeyType key = this.GetKey(cache);
            this.DicByKey.Remove(key);
        }

        public override void Clear()
        {
            this.DicByKey.Clear();
        }

        public override void Change(CacheChangedArgs<CacheType> args)
        {
            dynamic key = this.GetKey(args.Cache);
            dynamic snapshotKey = this.GetKey(args.Snapshot);
            if(key != snapshotKey)
            {
                this.Remove(snapshotKey);
                this.Add(args.Cache);
            }
        }

        public virtual CacheType GetByKey(KeyType key)
        {
            this.EnableValidate();

            if (key == null)
            {
                return default(CacheType);
            }

            this.AcquireReaderLock();
            try
            {
                if (this.DicByKey.ContainsKey(key))
                {
                    return this.DicByKey[key];
                }
                return default(CacheType);
            }
            finally
            {
                this.ReleaseReaderLock();
            }
        }
        public virtual bool ContainsKey(KeyType key)
        {
            if (key == null)
            {
                return false;
            }

            this.AcquireReaderLock();
            try
            {
                return this.DicByKey.ContainsKey(key);
            }
            finally
            {
                this.ReleaseReaderLock();
            }
        }

        public override object Get(object key)
        {
            if (key == null)
            {
                return null;
            }

            if (key is KeyType)
            {
                return this.GetByKey((KeyType)key);
            }

            throw new ArgumentException(string.Format("不支持{0}类型获取", key.GetType().Name));
        }


        public int GetCacheCount()
        {
            return this.DicByKey.Count;
        }
    }
}
