using EIM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net;
using System.Threading;
using EIM.Data;
using EIM.Business;
using EIM.Data.Org;
using EIM.Business.Org;

namespace EIM.Core
{
    public class CacheProviderManager
    {
        public CacheProviderManager(BusinessModelProviderFactory businessModelProviderFactory)
        {
            this._lock = new object();
            this.BusinessModelProviderFactory = businessModelProviderFactory;

            this.CacheProviders = new List<ICacheProvider>();

            this.CreateCacheProvider<User, UserDataModel>();
        }

        object _lock;

        public BusinessModelProviderFactory BusinessModelProviderFactory { private set; get; }

        public List<ICacheProvider> CacheProviders { private set; get; }

        protected virtual T CreateCacheProvider<T>()
        {
            T cacheProvder = (T)Activator.CreateInstance(typeof(T), this.BusinessModelProviderFactory);
            this.CacheProviders.Add(cacheProvder as ICacheProvider);

            return cacheProvder;
        }

        protected virtual DatabaseCacheProvider<CacheType, ModelType> CreateCacheProvider<CacheType, ModelType>()
            where ModelType : class, IKeyProvider
            where CacheType : class, IKeyProvider
        {
            DatabaseCacheProvider<CacheType, ModelType> cacheProvider = null;

            Type cacheProviderType = ReflectionHelper.GetSingleSubclass<DatabaseCacheProvider<CacheType, ModelType>>(this.GetType().Assembly);
            if (cacheProviderType == null)
            {
                cacheProvider = new DatabaseCacheProvider<CacheType, ModelType>(this.BusinessModelProviderFactory);
            }
            else
            {
                cacheProvider = Activator.CreateInstance(cacheProviderType, this.BusinessModelProviderFactory) as DatabaseCacheProvider<CacheType, ModelType>;
            }

            if (cacheProvider == null)
            {
                throw new ArgumentNullException("cacheProvider");
            }

            this.CacheProviders.Add(cacheProvider);

            return cacheProvider;
        }

        public ICacheProvider GetCacheProvider<CacheType, ModelType>()
            where ModelType : class, IKeyProvider
            where CacheType : class, IKeyProvider
        {
            return this.CacheProviders.Find(provider =>
                {
                    return provider is DatabaseCacheProvider<CacheType, ModelType>;
                });
        }

        public IDatabaseCacheProvider GetCacheProvider<CacheType>() where CacheType : class
        {
            return this.CacheProviders.Find(provider =>
            {
                return provider is CacheProvider<CacheType> && provider is IDatabaseCacheProvider;
            }) as IDatabaseCacheProvider;
        }

        public IDatabaseCacheProvider GetCacheProvider(Type type)
        {
            return this.CacheProviders.Find(provider =>
            {
                return provider.GetType() == type && provider is IDatabaseCacheProvider;
            }) as IDatabaseCacheProvider;
        }

        public List<IDatabaseCacheProvider> GetDatabaseCacheProviders() 
        {
            return this.CacheProviders.Where(provider => provider is IDatabaseCacheProvider)
                .Select(provider => provider as IDatabaseCacheProvider)
                .ToList();
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
    }
}
