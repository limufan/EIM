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

    public class DataModelMapperFactory
    {
        public DataModelMapperFactory(CacheContainer cacheContainer, params Assembly[] assemblys)
        {
            this.CacheContainer = cacheContainer;
            if(assemblys != null)
            {
                this.Assemblys = new Assembly[assemblys.Length + 1];
                Array.Copy(assemblys, this.Assemblys, assemblys.Length);
            }
            else
            {
                this.Assemblys = new Assembly[1];
            }
            this.Assemblys[this.Assemblys.Length - 1] = typeof(DataModelMapperFactory).Assembly;
        }

        public CacheContainer CacheContainer { set; get; }

        public Assembly[] Assemblys { set; get; }

        public DataModelMapper<ResultType, SourceType> CreateMapper<ResultType, SourceType>()
        {
            DataModelMapper<ResultType, SourceType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<DataModelMapper<ResultType, SourceType>>(this.Assemblys);
            if (mapperType == null)
            {
                mapper = new DataModelMapper<ResultType, SourceType>(this.CacheContainer);
            }
            else
            {
                mapper = Activator.CreateInstance(mapperType, this.CacheContainer) as DataModelMapper<ResultType, SourceType>;
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            return mapper;
        }

        public TargetType Map<TargetType, SourceType>(SourceType source)
        {
            DataModelMapper<TargetType, SourceType> mapper = this.CreateMapper<TargetType, SourceType>();

            return mapper.Map(source);
        }

        public void Map<TargetType, SourceType>(TargetType target, SourceType source)
        {
            DataModelMapper<TargetType, SourceType> mapper = this.CreateMapper<TargetType, SourceType>();

            mapper.Map(target, source);
        }

    }

}
