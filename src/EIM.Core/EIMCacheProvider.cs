﻿using EIM.Data;
using EIM.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using EIM.Business;
using Newtonsoft.Json;
using EIM.Cache.CacheManagers;
using EIM.Cache;
using EIM.Data.DataModelProviders;

namespace EIM.Core
{
    public class EIMCacheProvider<CacheType, DataModelType> : CacheProvider<CacheType>
            where CacheType : class, ICacheRefreshable<CacheType>
            where DataModelType : class
    {

        public EIMCacheProvider(CacheContainer cacheContainer, params ICacheManager[] dependentManagers)
            : base(cacheContainer, dependentManagers)
        {
#if DEBUG
            this.MaxCount = ConfigurationManagerHelper.GetIntValue("DataLoadMaxCount");
            if (this.MaxCount == 0)
            {
                this.MaxCount = int.MaxValue;
            }
#endif
            this.DataModelProviderFactory = new EFDataModelProviderFactory();
            DataModelMapperFactory dataModelMapperFactory = new DataModelMapperFactory(cacheContainer);
            this.CacheMapper = dataModelMapperFactory.Create<CacheType, DataModelType>();
        }

#if DEBUG
        public int MaxCount { set; get; }
#endif

        public TCacheMapper<CacheType, DataModelType> CacheMapper { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }

        protected virtual DataModelProvider<DataModelType> CreateDataProvider()
        {
            return this.DataModelProviderFactory.CreateDataProvider<DataModelType>();
        }

        protected override List<CacheType> GetCaches()
        {
            object addLock = new object();
            IList<DataModelType> models = this.GetModels();

            List<CacheType> caches = models.AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount / 2)
                .Select(model => this.Map(model))
                .ToList();
            
            return caches;
        }

        public override CacheType GetCache(object key)
        {
            DataModelType model = null;
            using (DataModelProvider<DataModelType> dataProvider = this.CreateDataProvider())
            {
                model = dataProvider.SelectById(key);
            }

            return this.Map(model);
        }

        protected virtual CacheType AddCache(DataModelType model)
        {
            CacheType cache = this.Map(model);
            this.CacheManager.Add(cache);

            return cache;
        }

        protected virtual void WriteCheckLog(string message, Exception ex)
        {
            EIMLog.Logger.Info(message, ex);
        }

        public virtual IList<DataModelType> GetModels()
        {
            using (DataModelProvider<DataModelType> dataProvider = this.CreateDataProvider())
            {
                return dataProvider.GetModels();
            }
        }

        public virtual CacheType Map(DataModelType model)
        {
            return this.CacheMapper.Map(model);
        }
    }
}
