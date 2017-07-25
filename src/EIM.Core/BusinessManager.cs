using EIM.Business;
using EIM.Business.CacheManagers;
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
            : this(new EFDataModelProviderFactory())
        {
            
        }

        public BusinessManager(DataModelProviderFactory dataModelProviderFactory)
        {
            this.CacheContainer = new CacheContainer();
            this.DataModelProviderFactory = dataModelProviderFactory;
            this.BusinessModelProviderFactory = new BusinessModelProviderFactory(this.CacheContainer, dataModelProviderFactory);
            this.CacheProviderManager = new CacheProviderManager(this.BusinessModelProviderFactory);

            this.UserManager = new UserManager(this);

            this.DataModelMapperFactory = new DataModelMapperFactory(this.CacheContainer);
            this.BusinessModelMapperFactory = new BusinessModelMapperFactory(this.CacheContainer);
        }

        public CacheContainer CacheContainer { set; get; }

        public DataModelMapperFactory DataModelMapperFactory { set; get; }

        public BusinessModelMapperFactory BusinessModelMapperFactory { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }

        public BusinessModelProviderFactory BusinessModelProviderFactory { set; get; }

        public CacheProviderManager CacheProviderManager { set; get; }

        public UserManager UserManager { set; get; }
    }
}
