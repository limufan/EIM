using EIM.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data
{
    public class DataManager
    {
        public DataManager(CacheContainer cacheContainer)
        {
            this.CacheContainer = cacheContainer;
            this.DataModelMapperFactory = new DataModelMapperFactory(cacheContainer);
            this.DataModelProviderFactory = new EFDataModelProviderFactory();
        }

        public CacheContainer CacheContainer { set; get; }

        public DataModelMapperFactory DataModelMapperFactory { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }
    }
}
