using EIM.Business.Org;
using EIM.Cache;
using EIM.Data;
using EIM.Data.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core
{
    public class EIMCacheProviderManager: CacheProviderManager
    {
        public EIMCacheProviderManager(DataManager dataManager)
        {
            this.DataManager = dataManager;

            this.CreateCacheProvider<User, UserDataModel>();
        }

        public DataManager DataManager { private set; get; }

        protected virtual DatabaseCacheProvider<CacheType, ModelType> CreateCacheProvider<CacheType, ModelType>()
            where ModelType : class
            where CacheType : class, ICache<CacheType>
        {
            DatabaseCacheProvider<CacheType, ModelType> cacheProvider = null;

            Type cacheProviderType = ReflectionHelper.GetSingleSubclass<DatabaseCacheProvider<CacheType, ModelType>>(this.GetType().Assembly);
            if (cacheProviderType == null)
            {
                cacheProvider = new DatabaseCacheProvider<CacheType, ModelType>(this.DataManager);
            }
            else
            {
                cacheProvider = Activator.CreateInstance(cacheProviderType, this.DataManager) as DatabaseCacheProvider<CacheType, ModelType>;
            }

            if (cacheProvider == null)
            {
                throw new ArgumentNullException("cacheProvider");
            }

            this.CacheProviders.Add(cacheProvider);

            return cacheProvider;
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

        public ICacheProvider GetCacheProvider<CacheType, ModelType>()
            where ModelType : class
            where CacheType : class, ICache<CacheType>
        {
            return this.CacheProviders.Find(provider =>
            {
                return provider is DatabaseCacheProvider<CacheType, ModelType>;
            });
        }
    }
}
