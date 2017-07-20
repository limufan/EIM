using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.CacheManagers
{
    public class ByIdCacheIndex<T>: CacheIndex<T>
        where T : class, IIdProvider
    {
        public ByIdCacheIndex(CacheManager<T> cacheManager):
            base(cacheManager)
        {
            this.DicById = new Dictionary<int, T>();
        }

        protected Dictionary<int, T> DicById { private set; get; }

        public virtual void Remove(int id)
        {
            T cache = this.GetById(id);
            if (cache != null)
            {
                this.Remove(cache);
            }
        }

        protected override void Add(T cache)
        {
            if (this.DicById.ContainsKey(cache.Id))
            {
                EIMLog.Logger.WarnFormat("{0} ID 重复ID: {1}", this.GetType().Name, cache.Id);
                return;
            }
            this.DicById.Add(cache.Id, cache);
        }

        protected override void Remove(T cache)
        {
            this.DicById.Remove(cache.Id);
        }

        protected override void Clear()
        {
            this.DicById.Clear();
        }

        public virtual T GetById(int id)
        {
            this.EnableValidate();

            this.AcquireReaderLock();
            try
            {
                if (this.DicById.ContainsKey(id))
                {
                    return this.DicById[id];
                }
                return default(T);
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

            if (key is int)
            {
                return this.GetById((int)key);
            }

            throw new ArgumentException(string.Format("不支持{0}类型获取", key.GetType().Name));
        }


        public int GetByIdCacheCount()
        {
            return this.DicById.Count;
        }
    }
}
