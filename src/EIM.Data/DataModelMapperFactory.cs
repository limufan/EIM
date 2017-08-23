using EIM.Business;
using EIM.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data
{

    public class DataModelMapperFactory: CacheMapperFactory
    {
        public DataModelMapperFactory(CacheContainer cacheContainer)
            : base(cacheContainer, typeof(DataModelMapperFactory).Assembly)
        {

        }
    }

}
