using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business
{
    public class ByGuidCodeCacheManager<T> : ByGuidCacheManager<T>
        where T : class, IGuidCodeProvider
    {
        public ByGuidCodeCacheManager()
        {
            this.DicByCode = new Dictionary<string, T>();
        }

        protected Dictionary<string, T> DicByCode { private set; get; }

        protected override void _Add(T cache)
        {
            base._Add(cache);
            this.AddCacheToDic(cache);
            if(cache is ICacheCodeChanged<T>)
            {
                (cache as ICacheCodeChanged<T>).CodeChanged += ByIdCodeCacheManager_CodeChanged;
            }
        }

        private void AddCacheToDic(T cache)
        {
            if (!this.DicByCode.ContainsKey(cache.Code))
            {
                this.DicByCode.Add(cache.Code, cache);
            }
            else
            {
                EIMLog.Logger.Info(string.Format("{0} Code 重复Guid: {1} Code: {2}", this.GetType().Name, cache.Guid, cache.Code));
            }
        }

        void ByIdCodeCacheManager_CodeChanged(T sender, CacheCodeChangedArgs args)
        {
            this.DicByCode.Remove(args.SnapshotCode);
            this.AddCacheToDic(sender);
        }

        protected override void _Remove(T cache)
        {
            base._Remove(cache);
            this.DicByCode.Remove(cache.Code);
        }

        public virtual T GetByCode(string code)
        {
            this.EnableValidate();
            if (string.IsNullOrEmpty(code))
            {
                return default(T);
            }

            this.Lock.AcquireReaderLock(10000);
            try
            {
                if (this.DicByCode.ContainsKey(code))
                {
                    return this.DicByCode[code];
                }
                return default(T);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
            }
        }

        public virtual bool ContainsCode(string code)
        {
            this.EnableValidate();
            if (string.IsNullOrEmpty(code))
            {
                return false;
            }

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return this.DicByCode.ContainsKey(code);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
            }
        }

        protected override void _Clear()
        {
            base._Clear();
            this.DicByCode.Clear();
        }

        public int GetByCodeCacheCount()
        {
            return this.DicByCode.Count;
        }
    }
}
