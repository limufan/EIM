using EIM.Cache.CacheManagers;
using EIM.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Cache
{
    public interface ICacheProvider
    {
        /// <summary>
        /// 加载缓存
        /// </summary>
        void Load(bool newThread);
    }

    public abstract class CacheProvider<CacheType> : ICacheProvider
            where CacheType : class, ICacheRefreshable<CacheType>
    {
        public CacheProvider(CacheContainer cacheContainer, params ICacheManager[] dependentManagers)
        {
            this._lock = new object();
            this._loading = false;
            this._watch = new Stopwatch();
            this.CacheManager = cacheContainer.GetManager<CacheType>();
            if (this.CacheManager == null)
            {
                throw new ArgumentNullException("this.CacheManager");
            }
            this.DependentManagers = new List<ICacheManager>();
            if (dependentManagers != null)
            {
                this.DependentManagers.AddRange(dependentManagers);
            }
        }
        object _lock;
        bool _loading;
        Stopwatch _watch;

        public CacheManager<CacheType> CacheManager { private set; get; }

        public List<ICacheManager> DependentManagers { private set; get; }

        public virtual void Load(bool newThread)
        {
            if (newThread)
            {
                Thread realodThread = new Thread(this.Load);
                realodThread.Start();
            }
            else
            {
                this.Load();
            }
        }

        protected virtual void Load()
        {
            try
            {

                if (this._loading)
                {
                    ConsoleHelper.WriteLine(this.GetType().Name + "检测到重复加载请求。");
                    return;
                }
                this._loading = true;

                this.OnLoading();

                List<CacheType> caches = this.GetCaches();

                foreach (CacheType cache in caches)
                {
                    this.CacheManager.Add(cache);
                }
#if DEBUG
                //测试模拟加载延迟
                if (caches.Count < 500)
                {
                    Thread.Sleep(200);
                }
#endif

                this.OnLoaded();

            }
            catch (Exception ex)
            {
                EIMLog.Logger.Error(ex.Message, ex);
                throw;
            }
            finally
            {
                this._loading = false;
            }
        }

        protected virtual void OnLoading()
        {
            this._watch.Start();
            ConsoleHelper.WriteLine(this.GetType().Name + "正在加载数据......");

            this.CacheManager.Clear();
            this.CacheManager.Status = CacheStatus.Loading;

            foreach (ICacheManager manager in this.DependentManagers)
            {
                //等10分钟
                for (int i = 0; i < 300; i++)
                {
                    if (!manager.Enable)
                    {
                        ConsoleHelper.WriteLine(string.Format("{0}等待依赖项{1}加载数据", this.GetType().Name, manager.GetType().Name));
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        break;
                    }
                }

                if (!manager.Enable)
                {
                    throw new EIMException(string.Format("{0}等待依赖项{1}超时!", this.GetType().Name, manager.GetType().Name));
                }
            }
        }

        protected virtual void OnLoaded()
        {
            this.CacheManager.Status = CacheStatus.Enable;

            this._watch.Stop();
            int count = this.CacheManager.Count();
            Console.WriteLine(string.Format("{0} 加载了{1}条数据, 用时: {2}", this.GetType().Name, count, this._watch.Elapsed.TotalSeconds));
        }

        public virtual void Refresh(object key)
        {
            CacheType newCache = this.GetCache(key);
            CacheType cache = this.CacheManager.Get(key) as CacheType;
            if (cache == null)
            {
                this.CacheManager.Add(newCache);
            }
            else
            {
                CacheType snapshot = cache.Refresh(newCache);
                this.CacheManager.Change(cache, snapshot);
            }
        }

        protected abstract List<CacheType> GetCaches();

        public abstract CacheType GetCache(object key);
    }
}
