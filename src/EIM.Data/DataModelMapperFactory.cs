using EIM.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data
{

    public class DataModelMapperFactory
    {
        public DataModelMapperFactory(CacheContainer cacheContainer)
        {
            this.CacheContainer = cacheContainer;
        }

        public CacheContainer CacheContainer { set; get; }

        public DataModelMapper<MappedType, ModelType> CreateMapper<MappedType, ModelType>()
        {
            DataModelMapper<MappedType, ModelType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<DataModelMapper<MappedType, ModelType>>(typeof(DataModelMapperFactory).Assembly);
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
