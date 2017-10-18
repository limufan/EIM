using EIM.Business;
using EIM.Cache.CacheManagers;
using EIM.Core.BusinessManagers;
using EIM.Core.Events;
using EIM.Core.MessageMangers;
using EIM.Data;
using EIM.Data.DataModelProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core
{
    public class BusinessManager
    {
        public BusinessManager()
        {
            this.EventManager = new EventManager();
            this.CacheContainer = new EIMCacheContainer(this.EventManager);
            this.MessageManager = new MessageManager(this.EventManager);
            this.MapperFactory = new DataModelMapperFactory(this.CacheContainer);
            this.DataModelProviderFactory = new EFDataModelProviderFactory();

            this.CacheProviderManager = new EIMCacheProviderManager(this.CacheContainer);
            this.CacheProviderManager.Load();

            this.CacheSyncService = new CacheSyncService(this.CacheProviderManager);
            this.CacheSyncService.Start();
        }

        public EIMCacheContainer CacheContainer { set; get; }

        public DataModelMapperFactory MapperFactory { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }

        public EIMCacheProviderManager CacheProviderManager { set; get; }

        public EventManager EventManager { set; get; }

        public MessageManager MessageManager { set; get; }

        public CacheSyncService CacheSyncService { set; get; }
    }
}
