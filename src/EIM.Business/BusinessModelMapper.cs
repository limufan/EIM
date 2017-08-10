using EIM.Business;
using EIM.Cache.CacheManagers;
using EIM.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM.Business
{
    public class BusinessModelMapper : CacheMapper
    {
        public BusinessModelMapper(CacheContainer cacheContainer)
            : base(cacheContainer)
        {
            
        }

        protected override bool Map(object source, Type resultType, out object result)
        {
            Type sourceType = source.GetType();
            //if (this.StringToDepartment(source, resultType, out result))
            //{
            //    return true;
            //}

            return base.Map(source, resultType, out result);
        }

        //private bool StringToDepartment(object source, Type resultType, out object result)
        //{
        //    result = null;
        //    if (source is string && resultType == typeof(Department))
        //    {
        //        string orgId = source as string;
        //        result = this.CacheContainer.DepartmentManager.GetByOrgId(orgId);
        //        return true;
        //    }
        //    return false;
        //}        
    }

    public class TBusinessModelMapper<MapType, SourceType> : BusinessModelMapper
    {
        public TBusinessModelMapper(CacheContainer cacheContainer)
            : base(cacheContainer)
        {
            
        }
    }
}
