using EIM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EIM.Business;

namespace EIM.Data
{
    public class DataModelMapperFactory
    {
        public DataModelMapperFactory(BusinessManager businessManager)
        {
            this.BusinessManager = businessManager;
        }

        public BusinessManager BusinessManager { set; get; }

        public DataModelMapper<MappedType, ModelType> CreateMapper<MappedType, ModelType>()
        {
            DataModelMapper<MappedType, ModelType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<DataModelMapper<MappedType, ModelType>>(typeof(DataModelMapperFactory).Assembly);
            if (mapperType == null)
            {
                mapper = new DataModelMapper<MappedType, ModelType>(this.BusinessManager);
            }
            else
            {
                mapper = Activator.CreateInstance(mapperType, this.BusinessManager) as DataModelMapper<MappedType, ModelType>;
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            return mapper;
        }

    }

    public interface IDataModelMapper
    {

    }

    public class DataModelMapper<MappedType, ModelType> : IDataModelMapper
    {
        public DataModelMapper(BusinessManager businessManager)
        {
            this.BusinessManager = businessManager;
        }

        public BusinessManager BusinessManager { set; get; }

        public virtual MappedType Map(ModelType model)
        {
            if (model == null)
            {
                return default(MappedType);
            }
            ConstructorInfo cotr = ReflectionHelper.GetConstructor(typeof(MappedType));
            ParameterInfo[] cotrParams = cotr.GetParameters();
            if (cotrParams.Length > 1)
            {
                throw new Exception("无法加载多个参数列表的对象!");
            }
            else if (cotrParams.Length == 1)
            {
                object infoArgs = Activator.CreateInstance(cotrParams[0].ParameterType);
                this.Map(infoArgs, model);
                MappedType obj = this.BusinessManager.ObjectMapper.Map<MappedType>(infoArgs);
                return obj;
            }
            else
            {
                MappedType obj = this.BusinessManager.ObjectMapper.Map<MappedType>(model);
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
            this.BusinessManager.ObjectMapper.Map(info, model);
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
            this.BusinessManager.ObjectMapper.Map(model, info);
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
