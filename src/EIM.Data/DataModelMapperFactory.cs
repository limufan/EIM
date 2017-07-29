using EIM.Business;
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

        public DataModelMapper<MappedType, ModelType> CreateMapper<MappedType, ModelType>()
        {
            DataModelMapper<MappedType, ModelType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<DataModelMapper<MappedType, ModelType>>(this.Assemblys);
            if (mapperType == null)
            {
                mapper = new DataModelMapper<MappedType, ModelType>(this.CacheContainer);
            }
            else
            {
                mapper = Activator.CreateInstance(mapperType, this.CacheContainer) as DataModelMapper<MappedType, ModelType>;
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            return mapper;
        }

    }

}
