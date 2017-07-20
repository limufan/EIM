using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM.Core
{
    public class BusinessModelMapper : ObjectMapper
    {
        public BusinessModelMapper(BusinessManager businessMapper)
            : base()
        {
            this._businessMapper = businessMapper;
        }
        BusinessManager _businessMapper;

        protected override bool Map(object source, Type resultType, out object result)
        {
            Type sourceType = source.GetType();
            //if (this.StringToDepartment(source, resultType, out result))
            //{
            //    return true;
            //}
            //else if (this.DepartmentToString(source, resultType, out result))
            //{
            //    return true;
            //}
            //else if (this.GuidToDepartment(source, resultType, out result))
            //{
            //    return true;
            //}
            //else if (this.DepartmentToGuid(source, resultType, out result))
            //{
            //    return true;
            //}
            //else if (this.KeyToObject(source, resultType, out result))
            //{
            //    return true;
            //}
             if (this.ObjectToKey(source, resultType, out result))
            {
                return true;
            }
            //else if (this.ModelToObject(source, resultType, out result))
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
        //        result = this._businessMapper.DepartmentManager.GetByOrgId(orgId);
        //        return true;
        //    }
        //    return false;
        //}

        //private bool DepartmentToString(object source, Type resultType, out object result)
        //{
        //    result = null;
        //    if (source is Department && resultType == typeof(string))
        //    {
        //        Department department = source as Department;
        //        result = department.OrganizationId;
        //        return true;
        //    }
        //    return false;
        //}

        //private bool GuidToDepartment(object source, Type resultType, out object result)
        //{
        //    result = null;
        //    if (source is Guid? && resultType == typeof(Department))
        //    {
        //        Guid? orgId = source as Guid?;
        //        if (orgId.HasValue)
        //        {
        //            result = this._businessMapper.DepartmentManager.GetByOrgId(orgId.Value.ToString());
        //        }
        //        return true;
        //    }
        //    else if (source is Guid && resultType == typeof(Department))
        //    {
        //        Guid orgId = (Guid)source;
        //        result = this._businessMapper.DepartmentManager.GetByOrgId(orgId.ToString());
        //        return true;
        //    }
        //    return false;
        //}

        //private bool DepartmentToGuid(object source, Type resultType, out object result)
        //{
        //    result = null;
        //    if (source is Department && resultType == typeof(Guid?))
        //    {
        //        Department department = source as Department;
        //        Guid guid;
        //        if(Guid.TryParse(department.OrganizationId, out guid))
        //        {
        //            result = guid;
        //        }
        //        return true;
        //    }
        //    else if (source is Department && resultType == typeof(Guid))
        //    {
        //        Department department = source as Department;
        //        Guid guid;
        //        if (Guid.TryParse(department.OrganizationId, out guid))
        //        {
        //            result = guid;
        //        }
        //        return true;
        //    }
        //    return false;
        //}

        //private bool KeyToObject(object source, Type resultType, out object result)
        //{
        //    result = null;

        //    if ((source is string || source is int) && !ReflectionHelper.IsIList(resultType))
        //    {
        //        if (this._businessMapper.Contains(resultType))
        //        {
        //            object key = source;
        //            result = this._businessMapper.Get(key, resultType);
        //            return true;
        //        }
        //    }
        //    else if (source is string && ReflectionHelper.IsIList(resultType))
        //    {
        //        Type resultItemType = ReflectionHelper.GetCollectionItemType(resultType);
        //        if (this._businessMapper.Contains(resultItemType))
        //        {
        //            ICacheManager manager = this._businessMapper.GetManager(resultItemType);
        //            string formatedKey = source as string;
        //            string[] keys = formatedKey.Split(',');
        //            result = Activator.CreateInstance(resultType);
        //            IList resultList = result as IList;
        //            foreach (string key in keys)
        //            {
        //                resultList.Add(manager.Get(key));
        //            }
        //            return true;
        //        }   
        //    }
        //    return false;
        //}

        private bool ObjectToKey(object source, Type resultType, out object result)
        {
            result = null;
            Type sourceType = source.GetType();

            if (!ReflectionHelper.IsIList(sourceType) && (resultType == typeof(string) || resultType == typeof(int)))
            {
                if(resultType == typeof(int) && source is IIdProvider)
                {
                    result = (source as IIdProvider).Id;
                    return true;
                }
                else if (resultType == typeof(string) && source is IGuidProvider)
                {
                    result = (source as IGuidProvider).Guid;
                    return true;
                }
                else if (resultType == typeof(string) && source is ICodeProvider)
                {
                    result = (source as ICodeProvider).Code;
                    return true;
                }

                return false;
            }
            else if (ReflectionHelper.IsIList<IIdProvider>(sourceType) && resultType == typeof(string))
            {
                List<object> keyList = new List<object>();
                IList list = source as IList;
                foreach (object obj in list)
                {
                    object key = key = (source as IIdProvider).Id;
                    keyList.Add(key);
                }
                result = string.Join(",", keyList);
                return true;
            }

            return false;
        }

        //private bool ModelToObject(object source, Type resultType, out object result)
        //{
        //    result = null;
        //    Type sourceType = source.GetType();

        //    if ((ReflectionHelper.Is<IIdProvider>(sourceType) || ReflectionHelper.Is<ICodeProvider>(sourceType) || ReflectionHelper.Is<IGuidProvider>(sourceType)) 
        //        && !this._businessMapper.Contains(sourceType) 
        //        && this._businessMapper.Contains(resultType))
        //    {
        //        if (source is IIdProvider)
        //        {
        //            IIdProvider idProviderSource = source as IIdProvider;
        //            result = this._businessMapper.Get(idProviderSource.ID, resultType);
        //            return true;
        //        }
        //        else if (source is IGuidProvider)
        //        {
        //            IGuidProvider guidProviderSource = source as IGuidProvider;
        //            result = this._businessMapper.Get(guidProviderSource.Guid, resultType);
        //            return true;
        //        }
        //        else if (source is ICodeProvider)
        //        {
        //            ICodeProvider codeProviderSource = source as ICodeProvider;
        //            result = this._businessMapper.Get(codeProviderSource.Code, resultType);
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }

    public class TBusinessModelMapper<MapType, SourceType> : BusinessModelMapper
    {
        public TBusinessModelMapper(BusinessManager businessManager)
            : base(businessManager)
        {
            this.BusinessManager = businessManager;
        }

        public BusinessManager BusinessManager { set; get; }
    }

    public class BusinessModelMapperFactory
    {
        public BusinessModelMapperFactory(BusinessManager businessManager)
        {
            this.BusinessManager = businessManager;
        }

        public BusinessManager BusinessManager { set; get; }

        public virtual TBusinessModelMapper<MapType, SourceType> Create<MapType, SourceType>()
        {
            TBusinessModelMapper<MapType, SourceType> mapper = null;

            Type mapperType = ReflectionHelper.GetSingleSubclass<TBusinessModelMapper<MapType, SourceType>>(this.GetTypes());
            if (mapperType == null)
            {
                mapper = new TBusinessModelMapper<MapType, SourceType>(this.BusinessManager);
            }
            else
            {
                mapper = Activator.CreateInstance(mapperType, this.BusinessManager) as TBusinessModelMapper<MapType, SourceType>;
            }

            if (mapper == null)
            {
                throw new ArgumentNullException("mapper");
            }

            return mapper;
        }

        public virtual BusinessModelMapper Create<T>()
        {
            return new BusinessModelMapper(this.BusinessManager);
        }

        protected virtual Type[] GetTypes()
        {
            return this.GetType().Assembly.GetExportedTypes();
        }
    }
}
