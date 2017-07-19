using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business
{
    public class ByIdCodeNameCacheManager<T> : ByIdCodeCacheManager<T>
        where T : class, IIdCodeNameProvider
    {
        public ByIdCodeNameCacheManager()
        {
            this._dicByName = new Dictionary<string, T>();
        }

        Dictionary<string, T> _dicByName;

        protected override void _Add(T cache)
        {
            base._Add(cache);
            if (!this._dicByName.ContainsKey(cache.UniqueName))
            {
                this._dicByName.Add(cache.UniqueName, cache);
            }
            else
            {
                //EIMLog.Logger.Info(string.Format("{0} Name 重复ID: {1} Code: {2} UniqueName: {3}", this.GetType().Name, cache.ID, cache.Code, cache.UniqueName));
            }
        }

        protected override void _Remove(T cache)
        {
            base._Remove(cache);
            this._dicByName.Remove(cache.UniqueName);
        }

        public T GetByName(string name)
        {
            this.EnableValidate();
            if (string.IsNullOrEmpty(name))
            {
                return default(T);
            }

            this.Lock.AcquireReaderLock(10000);
            try
            {
                if (this._dicByName.ContainsKey(name))
                {
                    return this._dicByName[name];
                }
                return default(T);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
            }
        }

        protected override void _Clear()
        {
            base._Clear();
            this._dicByName.Clear();
        }
    }
}
