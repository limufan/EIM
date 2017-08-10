using EIM.Cache.CacheManagers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Cache
{
    public class CacheMapper : ObjectMapper
    {
        public CacheMapper(CacheContainer cacheContainer)
            : base()
        {
            this.CacheContainer = cacheContainer;
        }

        public CacheContainer CacheContainer { set; get; }

        protected override bool Map(object source, Type resultType, out object result)
        {
            Type sourceType = source.GetType();
            if (this.KeyToCache(source, resultType, out result))
            {
                return true;
            }
            else if (this.CacheToKey(source, resultType, out result))
            {
                return true;
            }
            else if (this.ModelToCache(source, resultType, out result))
            {
                return true;
            }

            return base.Map(source, resultType, out result);
        }        

        private bool KeyToCache(object source, Type resultType, out object result)
        {
            result = null;

            if ((source is string || source is int) && !ReflectionHelper.IsIList(resultType))
            {
                if (this.CacheContainer.Contains(resultType))
                {
                    object key = source;
                    result = this.CacheContainer.Get(key, resultType);
                    return true;
                }
            }
            else if (source is string && ReflectionHelper.IsIList(resultType))
            {
                Type resultItemType = ReflectionHelper.GetCollectionItemType(resultType);
                if (this.CacheContainer.Contains(resultItemType))
                {
                    ICacheManager manager = this.CacheContainer.GetManager(resultItemType);
                    string formatedKey = source as string;
                    string[] keys = formatedKey.Split(',');
                    result = Activator.CreateInstance(resultType);
                    IList resultList = result as IList;
                    foreach (string key in keys)
                    {
                        resultList.Add(manager.Get(key));
                    }
                    return true;
                }
            }
            return false;
        }

        private bool CacheToKey(object source, Type resultType, out object result)
        {
            result = null;
            Type sourceType = source.GetType();

            if (!ReflectionHelper.IsIList(sourceType) && (resultType == typeof(string) || resultType == typeof(int)))
            {
                if (resultType == typeof(int) && source is IIdProvider)
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

        private bool ModelToCache(object source, Type resultType, out object result)
        {
            result = null;
            Type sourceType = source.GetType();

            if ((ReflectionHelper.Is<IIdProvider>(sourceType) || ReflectionHelper.Is<ICodeProvider>(sourceType) || ReflectionHelper.Is<IGuidProvider>(sourceType))
                && !this.CacheContainer.Contains(sourceType)
                && this.CacheContainer.Contains(resultType))
            {
                if (source is IIdProvider)
                {
                    IIdProvider idProviderSource = source as IIdProvider;
                    result = this.CacheContainer.Get(idProviderSource.Id, resultType);
                    return true;
                }
                else if (source is IGuidProvider)
                {
                    IGuidProvider guidProviderSource = source as IGuidProvider;
                    result = this.CacheContainer.Get(guidProviderSource.Guid, resultType);
                    return true;
                }
                else if (source is ICodeProvider)
                {
                    ICodeProvider codeProviderSource = source as ICodeProvider;
                    result = this.CacheContainer.Get(codeProviderSource.Code, resultType);
                    return true;
                }
            }

            return false;
        }
    }
}
