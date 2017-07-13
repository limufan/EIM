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

namespace EIM.Core
{
    public interface IDatabaseCacheProvider : ICacheProvider
    {
        bool LoadById(object id);

        bool DisableCheckLogger { get; set; }

        ReaderWriterLockedList<object> UnloadIdList { get; }
    }

    public abstract class DatabaseCacheProvider<CacheType, ModelType> : CacheProvider<CacheType>, IDatabaseCacheProvider
        where ModelType : class
        where CacheType: class
    {
        protected static Regex SqlKeywordRegex = new Regex("(update)|(insert)|(delete)", RegexOptions.IgnoreCase);
        static Dictionary<string, Regex> Mapped_Table_Regex = null;

        static DatabaseCacheProvider()
        {

        }

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
        private int _loadByIdCount;
        private int _loadByWhereCount;
        private DateTime _lastCheckTime;

        public ReaderWriterLockedList<object> UnloadIdList { private set; get; }

        public BusinessModelProviderFactory DataProviderFactory { set; get; }

        public DataModelMapper<CacheType, ModelType> CacheMapper { set; get; }

        public BusinessManager BusinessManager { set; get; }

        public CheckCacheResult<CacheType> CheckCacheResult { set; get; }

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

        public virtual bool LoadById(object id)
        {
            if(this._loadByIdCount ++ > 100000)
            {
                EIMLog.Logger.Warn(this.GetType().Name + " LoadById Count: " + this._loadByIdCount);
                this._loadByIdCount = 0;
            }

            LogStopwatch watch = new LogStopwatch(this.GetType().Name, "LoadById ID: " + id);
            watch.Start();
            
            try
            {
                using (BusinessModelProvider<CacheType, ModelType> dataProvider = this.CreateDataProvider())
                {
                    ModelType model = dataProvider.DataProvider.SelectById(id);
                    if (model != null)
                    {
                        this.AddOrUpdateCache(model);
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                this.UnloadIdList.Add(id);
                EIMLog.Logger.Error(this.GetType().Name + " LoadById: " + id + ex.Message, ex);
                throw;
            }
            finally
            {
                watch.Stop();
            }

            EIMLog.Logger.Warn(this.GetType().Name + " LoadById失败, id: " + id + "");
            return false;
        }

        public virtual bool LoadByWhere(Expression<Func<ModelType, bool>> expression)
        {
            if (this._loadByWhereCount++ > 100000)
            {
                EIMLog.Logger.Warn(this.GetType().Name + " LoadByWhere Count: " + this._loadByWhereCount);
                this._loadByWhereCount = 0;
            }

            LogStopwatch watch = new LogStopwatch(this.GetType().Name, "LoadByWhere By Expression");
            watch.Start();

            try
            {
                using (BusinessModelProvider<CacheType, ModelType> dataProvider = this.CreateDataProvider())
                {
                    IList<ModelType> models = dataProvider.DataProvider.SelectModels(expression);
                    foreach (ModelType model in models)
                    {
                        this.AddOrUpdateCache(model);
                    }
                    if (models.Count > 0)
                    {
                        return true;
                    }
                }
            }
            finally
            {
                watch.Stop();
            }
            return false;
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

        protected virtual CacheType AddOrUpdateCache(ModelType model)
        {
            CacheType cache = this.GetCache(model);
            if (cache == null)
            {
                cache = this.AddCache(model);
            }
            else
            {
                this.UpdateCache(model);
            }

            return cache;
        }

        protected virtual CacheType AddCache(ModelType model)
        {
            CacheType cache = this.Map(model);
            this.CacheManager.Add(cache);

            return cache;
        }

        public override void CheckRecentlyModifiedCache()
        {
            EIMLog.Logger.InfoFormat("{0} CheckRecentlyModifiedCache", this.GetType().Name);

            DateTime lastModifiedTime = this._lastCheckTime;

            try
            {

                List<CacheType> addOrUpdateCaches = new List<CacheType>();
                this._lastCheckTime = DateTime.Now;

                LogStopwatch watch = new LogStopwatch(this.GetType().Name + ".GetRecentlyModifiedModels", "timespan: " + (DateTime.Now - lastModifiedTime).TotalSeconds);
                watch.Start(1);

                IList<ModelType> models = this.GetRecentlyModifiedModels(lastModifiedTime);

                watch.Stop();

                foreach (ModelType model in models)
                {
                    addOrUpdateCaches.Add(this.AddOrUpdateCache(model));
                }

                CheckCacheResult<CacheType> checkCacheResult = new CheckCacheResult<CacheType>();
                checkCacheResult.AddOrUpdateCache = addOrUpdateCaches;
                checkCacheResult.RemoveCacheList = null;

                this.OnChecked(checkCacheResult);
                this.CheckCacheResult = checkCacheResult;

                EIMLog.Logger.InfoFormat("{0} CheckRecentlyModifiedCache End", this.GetType().Name);
            }
            catch
            {
                this._lastCheckTime = lastModifiedTime;
                throw;
            }
        }

        protected virtual IList<ModelType> GetRecentlyModifiedModels(DateTime modifiedTime)
        {
            return new List<ModelType>();
        }

        public override void CheckCache()
        {
            EIMLog.Logger.InfoFormat("{0} CheckCache", this.GetType().Name);

            List<CacheType> addOrUpdateCaches = new List<CacheType>();
            Dictionary<CacheType, CacheType> uncheckCacheList = this.CacheManager.CacheList.Distinct().ToDictionary(cache => cache);

            this._lastCheckTime = DateTime.Now;
            IList<ModelType> models = this.GetModels();

            foreach (ModelType model in models)
            {
                CacheType cache = this.GetCache(model);
                if (cache != null)
                {
                    uncheckCacheList.Remove(cache);
                }

                if (!this.CheckCache(model))
                {
                    CacheType addOrUpdateCache = this.AddOrUpdateCache(model);
                    addOrUpdateCaches.Add(addOrUpdateCache);
                    this.WriteCheckLog(this.GetType().Name + "AddOrUpdateCache:" + JsonConvert.SerializeObject(model), null);
                }
            }

            this.CacheManager.Remove(uncheckCacheList.Values.ToList());
            
            CheckCacheResult<CacheType> checkCacheResult = new CheckCacheResult<CacheType>();
            checkCacheResult.AddOrUpdateCache = addOrUpdateCaches;
            checkCacheResult.RemoveCacheList = uncheckCacheList.Values.ToList();
            this.OnChecked(checkCacheResult);

            this.WriteCheckEndLog(checkCacheResult);

            this.CheckCacheResult = checkCacheResult;
        }

        public virtual bool CheckCache(ModelType model)
        {
            CacheType cachedObj = this.GetCache(model);
            if (cachedObj == null)
            {
                this.WriteCheckLog(string.Format("{0} 检查到数据不一致, 内存中没有对象:{1}", this.GetType().Name, JsonConvert.SerializeObject(model)), null);
                return false;
            }

            CacheType reloadObj = this.Map(model);

            bool equals = false;
            try
            {
                equals = this.Compare(cachedObj, reloadObj);
            }
            catch (Exception ex)
            {
                this.WriteCheckLog(this.GetType().Name + "检查到数据不一致", ex);
            }

            if (!equals)
            {
                equals = this.RecheckCache(cachedObj, model);
            }

            return equals;
        }

        protected virtual bool RecheckCache(CacheType cachedObj, ModelType model)
        {
            bool equals = false;
            try
            {
                using (BusinessModelProvider<CacheType, ModelType> dataProvider = this.CreateDataProvider())
                {
                    dataProvider.DataProvider.Refresh(model);
                    CacheType reloadObj = this.Map(model);
                    equals = this.Compare(cachedObj, reloadObj);
                    if (equals)
                    {
                        this.WriteCheckLog(this.GetType().Name + "重新检查缓存正常:" + JsonConvert.SerializeObject(model), null);
                    }
                }
            }
            catch (Exception ex)
            {
                this.WriteCheckLog(this.GetType().Name + "检查到数据不一致", ex);
            }

            return equals;
        }

        protected virtual void WriteCheckLog(string message, Exception ex)
        {
            if (!this.DisableCheckLogger)
            {
                EIMLog.Logger.Info(message, ex);
            }
        }

        protected virtual void WriteCheckEndLog(CheckCacheResult<CacheType> checkCacheResult)
        {
            if (!this.DisableCheckLogger)
            {
                if (checkCacheResult.RemoveCacheList != null && checkCacheResult.RemoveCacheList.Count > 0)
                {
                    EIMLog.Logger.Warn(this.GetType().Name + " Remove Cache Count: " + checkCacheResult.RemoveCacheList.Count);
                }
                if (checkCacheResult.AddOrUpdateCache != null && checkCacheResult.AddOrUpdateCache.Count > 0)
                {
                    EIMLog.Logger.Warn(this.GetType().Name + " AddOrUpdateModels Count: " + checkCacheResult.AddOrUpdateCache.Count);
                }
            }
            EIMLog.Logger.InfoFormat("{0} CheckCache End", this.GetType().Name);
        }

        protected virtual bool Compare(CacheType cachedObj, CacheType reloadObj)
        {
            if (cachedObj is IPropertyComparable)
            {
                return (cachedObj as IPropertyComparable).CompareProperty(reloadObj);
            }
            else
            {
                ObjectComparer objectComparer = new ObjectComparer();
                return objectComparer.Compare(cachedObj, reloadObj);
            }
        }

        protected virtual void OnChecked(CheckCacheResult<CacheType> checkCacheResult)
        {
            
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

        protected abstract CacheType GetCache(ModelType model);

        protected abstract void UpdateCache(ModelType model);
    }
}
