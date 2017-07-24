using EIM.Business.CacheIndexes;
using EIM.Business.CacheManagers;
using EIM.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Business.CacheManagers
{
    public enum CacheStatus
    {
        Disable,
        Enable,
        Loading,
        Clearing,
        Cleared
    }

    public abstract class CacheManager<T> : ICacheManager
        where T : class
    {
        public CacheManager()
        {
            this._cacheList = new HashSet<T>();
            this.Lock = new ReaderWriterLock();
            this.CacheIndexes = new List<CacheIndex<T>>();
            this.CacheIndexes.AddRange(this.CreateCacheIndexes());
        }
        protected ReaderWriterLock Lock { private set; get; }

        HashSet<T> _cacheList;

        CacheStatus _status;
        public CacheStatus Status
        {
            set
            {
                this._status = value;
                if(this._status == CacheStatus.Enable)
                {
                    if (this.Enabled != null)
                    {
                        this.Enabled(this);
                    }

                    this.OnEnabled();
                }
            }
            get
            {
                return this._status;
            }
        }

        public bool Enable
        {
            get
            {
                return this._status == CacheStatus.Enable;
            }
        }

        public List<CacheIndex<T>> CacheIndexes { set; get; }

        public event TEventHandler<ICacheManager> Enabled;

        public event TEventHandler<T> Added;

        public event TEventHandler<T> Removed;

        public event TEventHandler Cleared;

        public virtual void EnableValidate()
        {
            if (!this.Enable)
            {
                throw new ObjectManagerUnableException(this.Status.ToString());
            }
        }

        protected virtual void OnEnabled()
        {

        }

        protected abstract List<CacheIndex<T>> CreateCacheIndexes();

        public virtual void Add(T cache)
        {
            this.Lock.AcquireWriterLock(10000);
            try
            {
                this._Add(cache);

                if(this.Added != null)
                {
                    this.Added(cache);
                }

                this.OnAdded(cache);
            }
            finally
            {
                this.Lock.ReleaseWriterLock();
            }
        }

        protected virtual void OnAdded(T cache)
        {

        }

        protected virtual void _Add(T cache)
        {
            this._cacheList.Add(cache);
        }

        public virtual void Remove(T cache)
        {
            this.EnableValidate();

            if (cache == null)
            {
                return;
            }
            this.Lock.AcquireWriterLock(10000);
            try
            {
                this._Remove(cache);

                if(this.Removed != null)
                {
                    this.Removed(cache);
                }
            }
            finally
            {
                this.Lock.ReleaseWriterLock();
            }
        }

        public virtual void Remove(List<T> removeCacheList)
        {
            this.EnableValidate();

            if (removeCacheList == null || removeCacheList.Count == 0)
            {
                return;
            }
            this.Lock.AcquireWriterLock(10000);
            try
            {
                foreach(T cache in removeCacheList)
                {
                    this._Remove(cache);
                }
            }
            finally
            {
                this.Lock.ReleaseWriterLock();
            }
        }

        protected virtual void _Remove(T cache)
        {
            this._cacheList.Remove(cache);
        }

        public virtual void Clear()
        {
            this.Status = CacheStatus.Clearing;

            this.Lock.AcquireWriterLock(10000);
            try
            {
                this._Clear();

                if(this.Cleared != null)
                {
                    this.Cleared();
                }
            }
            finally
            {
                this.Lock.ReleaseWriterLock();
            }

            this.Status = CacheStatus.Cleared;
        }

        protected virtual void _Clear()
        {
            this._cacheList.Clear();
        }

        public void ForEach(Action<T> action)
        {
            this.EnableValidate();

            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".ForEach", "");
            watch.Start();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                foreach (T t in this._cacheList)
                {
                    action(t);
                }
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
                watch.Stop();
            }
        }

        public List<T> Filtrate(ObjectFilter<T> filter, out int totalCount)
        {
            this.EnableValidate();

            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".Filtrate", "");
            watch.Start();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return filter.Filtrate(this._cacheList, out totalCount);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
                watch.Stop();
            }
        }

        public List<T> Filtrate(ObjectFilter<T> filter)
        {
            this.EnableValidate();

            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".Filtrate", "");
            watch.Start();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return filter.Filtrate(this._cacheList);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
                watch.Stop();
            }
        }

        public List<T> Where(Func<T, bool> filter)
        {
            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".Where", "");
            watch.Start();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return this._cacheList.Where(filter).ToList();
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
                watch.Stop();
            }
        }

        public bool Any(Func<T, bool> filter)
        {
            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".Any", "");
            watch.Start();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return this._cacheList.Any(filter);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
                watch.Stop();
            }
        }

        public virtual int Count()
        {
            this.EnableValidate();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return this._cacheList.Count;
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
            }
        }

        public virtual int Count(Func<T, bool> filter)
        {
            this.EnableValidate();

            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".Count", "");
            watch.Start();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return this._cacheList.Count(filter);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
                watch.Stop();
            }
        }

        public bool Contains(T cache)
        {
            this.EnableValidate();

            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".Contains", "");
            watch.Start();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return this._cacheList.Contains(cache);
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
                watch.Stop();
            }
        }

        public virtual List<object> Take(int count)
        {
            this.EnableValidate();

            LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".Take", "");
            watch.Start();

            this.Lock.AcquireReaderLock(10000);
            try
            {
                return this._cacheList.Take(count).Select(cache => cache as object).ToList();
            }
            finally
            {
                this.Lock.ReleaseReaderLock();
                watch.Stop();
            }
        }

        public virtual List<T> CacheList
        {
            get
            {
                this.Lock.AcquireReaderLock(10000);
                try
                {
                    return this._cacheList.ToList();
                }
                finally
                {
                    this.Lock.ReleaseReaderLock();
                }
            }
        }

        public bool IsCache(Type type)
        {
            return type == typeof(T);
        }


        public virtual object Get(object key)
        {
            foreach(CacheIndex<T> index in this.CacheIndexes)
            {
                object obj = index.Get(key);
                if(obj != null)
                {
                    return obj;
                }
            }

            return null;
        }

        public void AcquireReaderLock()
        {
            this.Lock.AcquireReaderLock(10000);
        }

        public void ReleaseReaderLock()
        {
            this.Lock.ReleaseReaderLock();
        }
    }
}
