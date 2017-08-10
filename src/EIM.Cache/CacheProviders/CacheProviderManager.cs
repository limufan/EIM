using EIM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net;
using System.Threading;

namespace EIM.Cache
{
    public class CacheProviderManager
    {
        public CacheProviderManager()
        {
            this._lock = new object();

            this.CacheProviders = new List<ICacheProvider>();
        }

        object _lock;

        public List<ICacheProvider> CacheProviders { private set; get; }

        protected virtual T CreateCacheProvider<T>()
        {
            T cacheProvder = (T)Activator.CreateInstance(typeof(T));
            this.CacheProviders.Add(cacheProvder as ICacheProvider);

            return cacheProvder;
        }

        private bool IsLoading { set; get; }

        public void LoadFromNewThread()
        {
            Thread realodThread = new Thread(this.Load);
            realodThread.Start();
        }

        public event TEventHandler<CacheProviderManager> Loading;
        public event TEventHandler<CacheProviderManager> Loaded;

        public void Load()
        {
            if (this.IsLoading)
            {
                return;
            }

            try
            {

                this.IsLoading = true;
                if (this.Loading != null)
                {
                    this.Loading(this);
                }

                lock (this._lock)
                {
                    ConsoleHelper.WriteLine("开始加载数据....");

                    foreach (ICacheProvider dataProvider in this.CacheProviders)
                    {
                        dataProvider.Load(false);
                    }
                }

                ConsoleHelper.WriteLine("加载完成");

                if (this.Loaded != null)
                {
                    this.Loaded(this);
                }

                GC.Collect();
            }
            finally
            {
                this.IsLoading = false;
            }
        }

        public ICacheProvider GetCacheProvider<CacheType, ModelType>()
            where ModelType : class
            where CacheType : class, ICache<CacheType>
        {
            return this.CacheProviders.Find(provider =>
            {
                return provider is ICacheProvider;
            });
        }
    }
}
