﻿using EIM.Core;
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

namespace EIM.Core
{
    public class CacheProviderManager
    {
        public CacheProviderManager(BusinessModelProviderFactory dataProviderFactory)
        {
            this._lock = new object();
            this.DataProviderFactory = dataProviderFactory;
            this.BusinessManager = dataProviderFactory.BusinessManager;

            this.CacheProviders = new List<ICacheProvider>();

            //this.ProvinceCacheProvider = this.CreateCacheProvider<ProvinceCacheProvider>();
        }

        object _lock;

        public BusinessModelProviderFactory DataProviderFactory { private set; get; }

        public BusinessManager BusinessManager { private set; get; }

        public List<ICacheProvider> CacheProviders { private set; get; }

        protected virtual T CreateCacheProvider<T>()
        {
            T cacheProvder = (T)Activator.CreateInstance(typeof(T), this.DataProviderFactory);
            this.CacheProviders.Add(cacheProvder as ICacheProvider);

            return cacheProvder;
        }

        public ICacheProvider GetCacheProvider<CacheType, ModelType>() 
            where ModelType : class
            where CacheType : class
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

        public DateTime LastCheckRecentlyModifyCacheTime { set; get; }

        public DateTime LastCheckCacheTime { set; get; }

        public DateTime LastCheckBaseCacheTime { set; get; }

        public void CheckRecentlyModifiedCache()
        {
            if (this.IsLoading)
            {
                return;
            }

            foreach (ICacheProvider dataProvider in this.CacheProviders)
            {
                dataProvider.CheckRecentlyModifiedCache();
            }

            this.LastCheckRecentlyModifyCacheTime = DateTime.Now;
        }

        public void CheckCache()
        {
            if (this.IsLoading)
            {
                return;
            }

            foreach (ICacheProvider dataProvider in this.CacheProviders)
            {
                dataProvider.CheckCache();
            }

            this.LastCheckCacheTime = DateTime.Now;
        }

        public void CheckBaseCache()
        {
            if (this.IsLoading)
            {
                return;
            }

            foreach (ICacheProvider dataProvider in this.CacheProviders)
            {
                if (dataProvider.IsBaseCache)
                {
                    dataProvider.CheckCache();
                }
            }

            GC.Collect();

            this.LastCheckBaseCacheTime = DateTime.Now;
        }
    }
}
