using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Cache
{
    public class CacheMapperFactory
    {
        public CacheMapperFactory(CacheContainer cacheContainer, params Assembly[] assemblys)
        {
            this.CacheContainer = cacheContainer;
            this.MapperTypes = this.GetMapperTypes(assemblys).ToArray();
        }

        public CacheContainer CacheContainer { set; get; }

        public Type[] MapperTypes { set; get; }

        protected virtual List<Type> GetMapperTypes(Assembly[] assemblys)
        {
            List<Type> mapperTypes = new List<Type>();
            if (assemblys != null && assemblys.Length > 0)
            {
                mapperTypes.AddRange(ReflectionHelper.GetSubclass<CacheMapper>(assemblys));
            }
            mapperTypes.AddRange(ReflectionHelper.GetSubclass<CacheMapper>(typeof(CacheMapperFactory).Assembly));
            return mapperTypes;
        }

        public virtual TCacheMapper<MapType, SourceType> Create<MapType, SourceType>()
        {
            TCacheMapper<MapType, SourceType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<TCacheMapper<MapType, SourceType>>(this.MapperTypes);
            if (mapperType == null)
            {
                mapper = new TCacheMapper<MapType, SourceType>(this.CacheContainer);
            }
            else
            {
                mapper = Activator.CreateInstance(mapperType, this.CacheContainer) as TCacheMapper<MapType, SourceType>;
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            return mapper;
        }

        public TargetType Map<TargetType, SourceType>(SourceType source)
        {
            TCacheMapper<TargetType, SourceType> mapper = this.Create<TargetType, SourceType>();

            return mapper.Map(source);
        }

        public void Map<TargetType, SourceType>(TargetType target, SourceType source)
        {
            TCacheMapper<TargetType, SourceType> mapper = this.Create<TargetType, SourceType>();

            mapper.Map(target, source);
        }
    }
}
