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
        public EIMCacheProviderManager(CacheContainer cacheContainer)
        {
            this.CacheContainer = cacheContainer;

            this.CreateCacheProvider<User, UserDataModel>();
        }

        public CacheContainer CacheContainer { private set; get; }

        protected virtual EIMCacheProvider<CacheType, ModelType> CreateCacheProvider<CacheType, ModelType>()
            where ModelType : class
            where CacheType : class, ICache<CacheType>
        {
            EIMCacheProvider<CacheType, ModelType> cacheProvider = null;

            Type cacheProviderType = ReflectionHelper.GetSingleSubclass<EIMCacheProvider<CacheType, ModelType>>(this.GetType().Assembly);
            if (cacheProviderType == null)
            {
                cacheProvider = new EIMCacheProvider<CacheType, ModelType>(this.CacheContainer);
            }
            else
            {
                cacheProvider = Activator.CreateInstance(cacheProviderType, this.CacheContainer) as EIMCacheProvider<CacheType, ModelType>;
            }

            if (cacheProvider == null)
            {
                throw new ArgumentNullException("cacheProvider");
            }

            this.CacheProviders.Add(cacheProvider);

            return cacheProvider;
        }
    }
}
