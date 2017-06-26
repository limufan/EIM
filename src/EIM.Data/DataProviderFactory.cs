using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIM.Core;

namespace EIM.Data
{
    public class DataProviderFactory
    {
        public DataProviderFactory(CacheManagerContainer cacheManagerContainer)
        {
            this.CacheManagerContainer = cacheManagerContainer;
            this.DataProviders = new List<IDataProvider>();
            this.DataProviderTypes = ReflectionHelper.GetSubclass<IDataProvider>(this.GetType().Assembly);
        }

        public CacheManagerContainer CacheManagerContainer { private set; get; }

        public List<IDataProvider> DataProviders { private set; get; }

        public Type[] DataProviderTypes { set; get; }

        public EIMDbContext CreateDbContext(string code)
        {
            EIMDbContext context = new EIMDbContext();
            return context;
        }
        
        public virtual T CreateDataProvider<T>() where T: IDataProvider
        {
            EIMDbContext context = this.CreateDbContext(typeof(T).Name);

            return this.CreateDataProvider<T>(context);
        }

        public virtual T CreateDataProvider<T>(EIMDbContext context) where T : IDataProvider
        {
            T dataProvder = (T)this.CreateDataProvider(typeof(T), context);

            return dataProvder;
        }

        public virtual CacheDataProvider<CacheType, ModelType> CreateDataProvider<CacheType, ModelType>() where ModelType : class
        {
            CacheDataProvider<CacheType, ModelType> dataProvider = this.CreateSubclassDataProvider<CacheDataProvider<CacheType, ModelType>>();

            return dataProvider;
        }

        public virtual DataProvider<ModelType> CreateDataProviderByModelType<ModelType>() where ModelType : class
        {
            DataProvider<ModelType> dataProvider = this.CreateSubclassDataProvider<DataProvider<ModelType>>();

            return dataProvider;
        }

        protected virtual T CreateSubclassDataProvider<T>() where T : class
        {
            Type dataProviderType = ReflectionHelper.GetSingleSubclass<T>(this.DataProviderTypes);
            if (dataProviderType == null)
            {
                throw new ArgumentException("无法获取DataProvider类型");
            }

            T dataProvider = this.CreateDataProvider(dataProviderType) as T;

            if (dataProvider == null)
            {
                throw new ArgumentException("无法创建DataProvider");
            }

            return dataProvider;
        }

        protected virtual object CreateDataProvider(Type type)
        {
            EIMDbContext context = this.CreateDbContext(type.Name);

            return this.CreateDataProvider(type, context);
        }

        protected virtual object CreateDataProvider(Type type, EIMDbContext context) 
        {
            IDataProvider dataProvider =  Activator.CreateInstance(type, this.CacheManagerContainer, context) as IDataProvider;

            return dataProvider;
        }
    }
}
