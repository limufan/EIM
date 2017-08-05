using EIM.Business;
using EIM.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core
{
    public class BusinessModelMapperFactory
    {
        public BusinessModelMapperFactory(CacheContainer cacheContainer, params Assembly[] assemblys)
        {
            this.CacheContainer = cacheContainer;
            List<Type> mapperTypes = new List<Type>();
            if (assemblys != null && assemblys.Length > 0)
            {
                mapperTypes.AddRange(ReflectionHelper.GetSubclass<BusinessModelMapper>(assemblys));
            }
            mapperTypes.AddRange(ReflectionHelper.GetSubclass<BusinessModelMapper>(typeof(BusinessModelMapperFactory).Assembly));
            this.MapperTypes = mapperTypes.ToArray();
        }

        public CacheContainer CacheContainer { set; get; }

        public Type[] MapperTypes { set; get; }

        public virtual TBusinessModelMapper<MapType, SourceType> Create<MapType, SourceType>()
        {
            TBusinessModelMapper<MapType, SourceType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<TBusinessModelMapper<MapType, SourceType>>(this.MapperTypes);
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
