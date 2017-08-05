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
using EIM.Cache.CacheManagers;

namespace EIM.Core
{
    public interface IDatabaseCacheProvider : ICacheProvider
    {
        bool DisableCheckLogger { get; set; }

        ReaderWriterLockedList<object> UnloadIdList { get; }
    }

    public class DatabaseCacheProvider<BusinessType, ModelType> : CacheProvider<BusinessType>, IDatabaseCacheProvider
            where BusinessType : class
            where ModelType : class
    {

        public DatabaseCacheProvider(DataManager dataManager, params ICacheManager[] dependentManagers)
            : base(dataManager.CacheContainer, dependentManagers)
        {
#if DEBUG
            this.MaxCount = ConfigurationManagerHelper.GetIntValue("DataLoadMaxCount");
            if (this.MaxCount == 0)
            {
                this.MaxCount = int.MaxValue;
            }
#endif
            this.DataManager = dataManager;
            this.DataModelProviderFactory = this.DataManager.DataModelProviderFactory;
            this.CacheMapper = dataManager.DataModelMapperFactory.CreateMapper<BusinessType, ModelType>();
            this.UnloadIdList = new ReaderWriterLockedList<object>();
        }

#if DEBUG
        public int MaxCount { set; get; }
#endif
        private DateTime _lastCheckTime;

        public ReaderWriterLockedList<object> UnloadIdList { private set; get; }

        public DataManager DataManager { set; get; }

        public DataModelMapper<BusinessType, ModelType> CacheMapper { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }

        public bool DisableCheckLogger { set; get; }

        protected virtual DataModelProvider<ModelType> CreateDataProvider()
        {
            return this.DataModelProviderFactory.CreateDataProvider<ModelType>();
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this._lastCheckTime = DateTime.Now;
        }

        protected override List<BusinessType> GetCaches()
        {
            object addLock = new object();
            IList<ModelType> models = this.GetModels();

            List<BusinessType> caches = models.AsParallel()
                .WithDegreeOfParallelism(Environment.ProcessorCount / 2)
                .Select(model => this.Map(model))
                .ToList();
            
            return caches;
        }

        protected virtual BusinessType AddCache(ModelType model)
        {
            BusinessType cache = this.Map(model);
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
            using (DataModelProvider<ModelType> dataProvider = this.CreateDataProvider())
            {
                return dataProvider.GetModels();
            }
        }

        public virtual BusinessType Map(ModelType model)
        {
            return this.CacheMapper.Map(model);
        }
    }
}
