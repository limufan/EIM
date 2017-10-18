using EIM.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Services
{
    public class ServiceModelMapperFactory: CacheMapperFactory
    {
        public ServiceModelMapperFactory(CacheContainer cacheContainer)
            :base(cacheContainer, typeof(ServiceModelMapperFactory).Assembly)
        {
            
        }
    }
}
