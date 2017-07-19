using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business
{
    public class ByIdCodeLongCodeCacheManager<T> : ByIdCodeCacheManager<T>
        where T : class, IIdCodeLongCodeProvider
    {
        public ByIdCodeLongCodeCacheManager()
        {
            this._dicByLongCode = new Dictionary<string, T>();
        }

        Dictionary<string, T> _dicByLongCode;

        protected override void _Add(T cache)
        {
            base._Add(cache);
            if (!this._dicByLongCode.ContainsKey(cache.LongCode))
            {
                this._dicByLongCode.Add(cache.LongCode, cache);
            }
            else
            {
                EIMLog.Logger.Info(string.Format("{0} LongCode 重复ID: {1} Code: {2} LongCode: {3}", this.GetType().Name, cache.ID, cache.Code, cache.LongCode));
            }
        }

        protected override void _Remove(T cache)
        {
            base._Remove(cache);
            this._dicByLongCode.Remove(cache.LongCode);
        }

        public T GetByLongCode(string longCode)
        {
            this.EnableValidate();
            if (string.IsNullOrEmpty(longCode))
            {
                return default(T);
            }

            this.Lock.AcquireReaderLock(10000);
            try
            {
                if (this._dicByLongCode.ContainsKey(longCode))
                {
                    return this._dicByLongCode[longCode];
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
            this._dicByLongCode.Clear();
        }
    }
}
