using EIM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM.Data
{
    public class DataModelCacheMapperFactory
    {
        public static DataModelCacheMapper<CacheType, ModelType> CreateMapper<CacheType, ModelType>(CacheManagerContainer cacheManagerContainer)
        {
            DataModelCacheMapper<CacheType, ModelType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<DataModelCacheMapper<CacheType, ModelType>>(typeof(DataModelCacheMapperFactory).Assembly);
            if (mapperType == null)
            {
                mapper = new DataModelCacheMapper<CacheType, ModelType>(cacheManagerContainer);
            }
            else
            {
                mapper = Activator.CreateInstance(mapperType, cacheManagerContainer) as DataModelCacheMapper<CacheType, ModelType>;
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            return mapper;
        }

    }

    public interface IDataModelCacheMapper
    {

    }

    public class DataModelCacheMapper<CacheType, ModelType> : IDataModelCacheMapper
    {
        public DataModelCacheMapper(CacheManagerContainer cacheManagerContainer)
        {
            this.CacheManagerContainer = cacheManagerContainer;
        }

        public CacheManagerContainer CacheManagerContainer { set; get; }

        public virtual CacheType Map(ModelType model)
        {
            if (model == null)
            {
                return default(CacheType);
            }
            ConstructorInfo cotr = ReflectionHelper.GetConstructor(typeof(CacheType));
            ParameterInfo[] cotrParams = cotr.GetParameters();
            if (cotrParams.Length > 1)
            {
                throw new Exception("无法加载多个参数列表的对象!");
            }
            else if (cotrParams.Length == 1)
            {
                object infoArgs = Activator.CreateInstance(cotrParams[0].ParameterType);
                this.Map(infoArgs, model);
                CacheType obj = this.CacheManagerContainer.ObjectMapper.Map<CacheType>(infoArgs);
                return obj;
            }
            else
            {
                CacheType obj = this.CacheManagerContainer.ObjectMapper.Map<CacheType>(model);
                return obj;
            }
        }

        /// <summary>
        /// 将model映射成Info
        /// </summary>
        /// <typeparam name="InfoType"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual InfoType Map<InfoType>(ModelType model)
        {
            if (model == null)
            {
                return default(InfoType);
            }

            InfoType info = Activator.CreateInstance<InfoType>();
            this.Map(info, model);

            return info;
        }

        /// <summary>
        /// 将model的字段映射到info上
        /// </summary>
        /// <param name="info"></param>
        /// <param name="model"></param>
        public virtual void Map(object info, ModelType model)
        {
            this.CacheManagerContainer.ObjectMapper.Map(info, model);
        }

        /// <summary>
        /// 将info映射成model
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public virtual ModelType Map(object info)
        {
            if (info == null)
            {
                return default(ModelType);
            }
            ModelType model = Activator.CreateInstance<ModelType>();
            this.Map(model, info);

            return model;
        }

        /// <summary>
        /// 将info的字段映射到model上
        /// </summary>
        /// <param name="model"></param>
        /// <param name="info"></param>
        public virtual void Map(ModelType model, object info)
        {
            this.CacheManagerContainer.ObjectMapper.Map(model, info);
        }
    }

    //public class DepartmentAreaDataModelCacheMapper : DataModelCacheMapper<DepartmentArea, DepartmentAreaDataModel>
    //{
    //    public DepartmentAreaDataModelCacheMapper(CacheManagerContainer cacheManagerContainer)
    //        : base(cacheManagerContainer)
    //    {

    //    }
        
    //    public override void Map(object info, DepartmentAreaDataModel model)
    //    {
    //        base.Map(info, model);

    //        if (info is DepartmentAreaBaseInfo)
    //        {
    //            DepartmentAreaBaseInfo baseInfo = info as DepartmentAreaBaseInfo;
    //            baseInfo.Quyu = this.CacheManagerContainer.LocationManager.GetQuyuByCode(model.Code);
    //        }
    //    }
    //}
}
