using EIM.Core;
using EIM.Exceptions;
using EIM.Data;
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
using EIM.Business.CacheManagers;

namespace EIM.Core
{
    public interface IDatabaseCacheProvider : ICacheProvider
    {
        bool DisableCheckLogger { get; set; }

        ReaderWriterLockedList<object> UnloadIdList { get; }
    }

    public class DatabaseCacheProvider<CacheType, ModelType> : CacheProvider<CacheType>, IDatabaseCacheProvider
        where ModelType : class
        where CacheType: class
    {

        public DatabaseCacheProvider(BusinessModelProviderFactory dataProviderFactory, params ICacheManager[] dependentManagers)
            : base(dataProviderFactory, dependentManagers)
        {
#if DEBUG
            this.MaxCount = ConfigurationManagerHelper.GetIntValue("DataLoadMaxCount");
            if (this.MaxCount == 0)
            {
                this.MaxCount = int.MaxValue;
            }
#endif
            this.DataProviderFactory = dataProviderFactory;
            this.BusinessManager = dataProviderFactory.BusinessManager;
            this.CacheMapper = dataProviderFactory.DataModelMapperFactory.CreateMapper<CacheType, ModelType>();
            this.UnloadIdList = new ReaderWriterLockedList<object>();
        }

#if DEBUG
        public int MaxCount { set; get; }
#endif
        private DateTime _lastCheckTime;

        public ReaderWriterLockedList<object> UnloadIdList { private set; get; }

        public BusinessModelProviderFactory DataProviderFactory { set; get; }

        public DataModelMapper<CacheType, ModelType> CacheMapper { set; get; }

        public BusinessManager BusinessManager { set; get; }

        public bool DisableCheckLogger { set; get; }

        protected virtual BusinessModelProvider<CacheType, ModelType> CreateDataProvider()
        {
            return this.DataProviderFactory.CreateDataProvider<CacheType, ModelType>();
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this._lastCheckTime = DateTime.Now;
        }

        protected override List<CacheType> GetCaches()
        {
            object addLock = new object();
            IList<ModelType> models = this.GetModels();

            List<CacheType> caches = models.AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount / 2)
                .Select(model => this.Map(model))
                .ToList();
            
            return caches;
        }

        protected virtual CacheType AddCache(ModelType model)
        {
            CacheType cache = this.Map(model);
            this.CacheManager.Add(cache);

            return cache;
        }

        protected virtual void WriteCheckLog(string message, Exception ex)
        {
            if (!this.DisableCheckLogger)
            {
                EIMLog.Logger.Info(message, ex);
            }
        }

        public virtual IList<ModelType> GetModels()
        {
            using (BusinessModelProvider<CacheType, ModelType> dataProvider = this.CreateDataProvider())
            {
                return dataProvider.DataProvider.GetModels();
            }
        }

        public virtual CacheType Map(ModelType model)
        {
            return this.CacheMapper.Map(model);
        }
    }
}
