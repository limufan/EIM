using EIM.Business;
using EIM.Cache.CacheManagers;
using EIM.Core.BusinessManagers;
using EIM.Data;
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
            this.CacheProviderManager = new EIMCacheProviderManager(this.CacheContainer);

            this.UserManager = new UserManager(this);

            this.DataModelMapperFactory = new DataModelMapperFactory(this.CacheContainer);
            this.BusinessModelMapperFactory = new BusinessModelMapperFactory(this.CacheContainer);

            this.CacheProviderManager.Load();
        }

        public EIMCacheContainer CacheContainer { set; get; }

        public DataModelMapperFactory DataModelMapperFactory { set; get; }

        public BusinessModelMapperFactory BusinessModelMapperFactory { set; get; }

        public EIMCacheProviderManager CacheProviderManager { set; get; }

        public UserManager UserManager { set; get; }
    }
}
