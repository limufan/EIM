using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace EIM.Business.CacheManagers
{
    public class DelayCacheManager<T> where T : class
    {
        public DelayCacheManager(CacheManager<T> cacheManager)
        {
            this.CacheManager = cacheManager;
            this.Lock = new ReaderWriterLock();
            this._refreshTime = DateTime.MinValue;
            this.ExpiredTime = new TimeSpan(0, 10, 0);
        }

        DateTime _refreshTime;
        List<T> _cacheList;
        protected ReaderWriterLock Lock { private set; get; }

        public TimeSpan ExpiredTime { set; get; }

        public CacheManager<T> CacheManager { set; get; }

        public virtual List<T> CacheList
        {
            get
            {
                if (this.Expired())
                {
                    this.RefreshCache();
                }

                return this._cacheList;
            }
        }

        public void RefreshCache()
        {
            this.Lock.AcquireReaderLock(10000);
            try
            {
                if (this.Expired())
                {
                    EIMLog.Logger.InfoFormat("{0}缓存过期", this.GetTypeName());
                    this._cacheList = this.DefaultSoft(this.CacheManager.CacheList);
                    this._refreshTime = DateTime.Now;
                }
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
            }
        }

        public bool Expired()
        {
            return this._cacheList == null || (DateTime.Now - this._refreshTime) > this.ExpiredTime;
        }

        public virtual int Count(Func<T, bool> filter)
        {
            LogStopwatch watch = new LogStopwatch(this.GetTypeName() + ".Count", "CacheList: " + this.CacheList.Count);
            watch.Start();

            try
            {
                return this.CacheList.Count(filter);
            }
            finally
            {
                watch.Stop();
            }
        }

        public void ForEach(Action<T> action)
        {
            LogStopwatch watch = new LogStopwatch(this.GetTypeName() + ".ForEach", "CacheList: " + this.CacheList.Count);
            watch.Start();

            try
            {
                foreach (T t in this.CacheList)
                {
                    action(t);
                }
            }
            finally
            {
                watch.Stop();
            }
        }

        public List<T> Filtrate(ObjectFilter<T> filter, out int totalCount)
        {
            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".Filtrate", "");
            watch.Start();

            try
            {
                return filter.Filtrate(this.CacheList, out totalCount);
            }
            finally
            {
                watch.Stop();
            }
        }

        private string GetTypeName()
        {
            return "DelayCacheManager." + typeof(T).Name;
        }

        protected virtual List<T> DefaultSoft(List<T> list)
        {
            return list;
        }
    }
}
