using EIM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using EIM.Business;
using EIM.Cache;

namespace EIM.Data
{
    public interface IDataModelMapper
    {

    }

    public class DataModelMapper<TargetType, SourceType> : IDataModelMapper
    {
        public DataModelMapper(CacheContainer cacheContainer)
        {
            this.BusinessModelMapper = new BusinessModelMapper(cacheContainer);
        }

        public BusinessModelMapper BusinessModelMapper { set; get; }

        public virtual TargetType Map(SourceType model)
        {
            if (model == null)
            {
                return default(TargetType);
            }
            ConstructorInfo cotr = ReflectionHelper.GetConstructor(typeof(TargetType));
            ParameterInfo[] cotrParams = cotr.GetParameters();
            if (cotrParams.Length > 1)
            {
                throw new Exception("无法加载多个参数列表的对象!");
            }
            else if (cotrParams.Length == 1)
            {
                object infoArgs = this.BusinessModelMapper.Map(model, cotrParams[0].ParameterType);

                TargetType obj = this.BusinessModelMapper.Map<TargetType>(infoArgs);
                return obj;
            }
            else
            {
                TargetType obj = this.BusinessModelMapper.Map<TargetType>(model);
                return obj;
            }
        }

        public void Map(TargetType target, SourceType source)
        {
            this.BusinessModelMapper.Map(target, source);
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
