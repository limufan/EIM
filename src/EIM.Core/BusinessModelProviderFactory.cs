using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIM.Business;
using EIM.Core;
using EIM.Data;

namespace EIM.Core
{
    public class BusinessModelProviderFactory
    {
        public BusinessModelProviderFactory(CacheContainer cacheContainer, DataModelProviderFactory dataProviderFactory)
        {
            this.DataModelProviderFactory = dataProviderFactory;
            this.CacheContainer = cacheContainer;
            this.DataModelMapperFactory = new DataModelMapperFactory(this.CacheContainer);
            this.DataProviders = new List<IBusinessModelProvider>();
            this.DataProviderTypes = ReflectionHelper.GetSubclass<IBusinessModelProvider>(this.GetType().Assembly);
        }

        public DataModelProviderFactory DataModelProviderFactory { private set; get; }

        public CacheContainer CacheContainer { private set; get; }

        public DataModelMapperFactory DataModelMapperFactory { private set; get; }

        public List<IBusinessModelProvider> DataProviders { private set; get; }

        public Type[] DataProviderTypes { set; get; }

        public virtual BusinessModelProvider<BusinessType, ModelType> CreateDataProvider<BusinessType, ModelType>() 
            where BusinessType : IKeyProvider
            where ModelType : class, IKeyProvider
        {
            DataModelProvider<ModelType> dataModelProvider = this.DataModelProviderFactory.CreateDataProviderByModelType<ModelType>();

            BusinessModelProvider<BusinessType, ModelType> dataProvider = this.CreateDataProvider<BusinessType, ModelType>(dataModelProvider);
            if(dataProvider == null)
            {
                dataProvider = new BusinessModelProvider<BusinessType, ModelType>(this.DataModelMapperFactory, dataModelProvider);
            }

            return dataProvider;
        }

        public virtual BusinessModelProvider<BusinessType, ModelType> CreateDataProvider<BusinessType, ModelType>(DataModelProvider<ModelType> dataModelProvider)
            where BusinessType : IKeyProvider
            where ModelType : class, IKeyProvider
        {
            BusinessModelProvider<BusinessType, ModelType> dataProvider = this.CreateSubclassDataProvider<BusinessModelProvider<BusinessType, ModelType>>(dataModelProvider);

            return dataProvider;
        }

        protected virtual T CreateSubclassDataProvider<T>(IDataModelProvider dataModelProvider) where T : class
        {
            Type dataProviderType = ReflectionHelper.GetSingleSubclass<T>(this.DataProviderTypes);
            if (dataProviderType == null)
            {
                return null;
            }

            T dataProvider = Activator.CreateInstance(dataProviderType, this.DataModelMapperFactory, dataModelProvider) as T;

            if (dataProvider == null)
            {
                throw new ArgumentException("无法创建DataProvider");
            }

            return dataProvider;
        }        
    }
}
