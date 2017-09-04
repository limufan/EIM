using EIM.Business;
using EIM.Cache.CacheManagers;
using EIM.Core.BusinessManagers;
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
            this.CacheContainer = new EIMCacheContainer();

            this.MapperFactory = new DataModelMapperFactory(this.CacheContainer);
            this.DataModelProviderFactory = new EFDataModelProviderFactory();

            this.UserManager = new UserManager(this);

            this.CacheProviderManager = new EIMCacheProviderManager(this.CacheContainer);
            this.CacheProviderManager.Load();
        }

        public EIMCacheContainer CacheContainer { set; get; }

        public DataModelMapperFactory MapperFactory { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }

        public EIMCacheProviderManager CacheProviderManager { set; get; }

        public UserManager UserManager { set; get; }
    }
}
