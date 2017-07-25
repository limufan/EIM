using EIM.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core
{
    public class BusinessModelMapperFactory
    {
        public BusinessModelMapperFactory(CacheContainer cacheContainer)
        {
            this.CacheContainer = cacheContainer;
        }

        public CacheContainer CacheContainer { set; get; }

        public virtual TBusinessModelMapper<MapType, SourceType> Create<MapType, SourceType>()
        {
            TBusinessModelMapper<MapType, SourceType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<TBusinessModelMapper<MapType, SourceType>>(this.GetTypes());
            if (mapperType == null)
            {
                mapper = new TBusinessModelMapper<MapType, SourceType>(this.CacheContainer);
            }
            else
            {
                mapper = Activator.CreateInstance(mapperType, this.CacheContainer) as TBusinessModelMapper<MapType, SourceType>;
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            return mapper;
        }

        public virtual BusinessModelMapper Create<T>()
        {
            return new BusinessModelMapper(this.CacheContainer);
        }

        protected virtual Type[] GetTypes()
        {
            return this.GetType().Assembly.GetExportedTypes();
        }
    }
}
