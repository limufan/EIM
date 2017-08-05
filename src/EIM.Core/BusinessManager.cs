using EIM.Business;
using EIM.Cache.CacheManagers;
using EIM.Core.BusinessModelManagers;
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
            this.DataManager = new DataManager(this.CacheContainer);
            this.DataModelProviderFactory = this.DataManager.DataModelProviderFactory;
            this.CacheProviderManager = new EIMCacheProviderManager(this.DataManager);

            this.UserManager = new UserManager(this);

            this.DataModelMapperFactory = new DataModelMapperFactory(this.CacheContainer);
            this.BusinessModelMapperFactory = new BusinessModelMapperFactory(this.CacheContainer);
        }

        public EIMCacheContainer CacheContainer { set; get; }

        public DataManager DataManager { set; get; }

        public DataModelMapperFactory DataModelMapperFactory { set; get; }

        public BusinessModelMapperFactory BusinessModelMapperFactory { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }

        public EIMCacheProviderManager CacheProviderManager { set; get; }

        public UserManager UserManager { set; get; }
    }
}
