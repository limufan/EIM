using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIM.Business;
using EIM.Core;

namespace EIM.Data
{
    public class BusinessModelProviderFactory
    {
        public BusinessModelProviderFactory(BusinessManager businessManager, DataModelProviderFactory dataProviderFactory)
        {
            this.DataProviderFactory = dataProviderFactory;
            this.BusinessManager = businessManager;
            this.DataModelMapperFactory = new DataModelMapperFactory(businessManager);
            this.DataProviders = new List<IBusinessModelProvider>();
            this.DataProviderTypes = ReflectionHelper.GetSubclass<IBusinessModelProvider>(this.GetType().Assembly);
        }

        public DataModelProviderFactory DataProviderFactory { private set; get; }

        public BusinessManager BusinessManager { private set; get; }

        public DataModelMapperFactory DataModelMapperFactory { private set; get; }

        public List<IBusinessModelProvider> DataProviders { private set; get; }

        public Type[] DataProviderTypes { set; get; }

        public virtual BusinessModelProvider<MappedType, ModelType> CreateDataProvider<MappedType, ModelType>() where ModelType : class
        {
            DataModelProvider<ModelType> dataModelProvider = this.DataProviderFactory.CreateDataProviderByModelType<ModelType>();

            BusinessModelProvider<MappedType, ModelType> dataProvider = this.CreateDataProvider<MappedType, ModelType>(dataModelProvider);

            return dataProvider;
        }

        public virtual BusinessModelProvider<MappedType, ModelType> CreateDataProvider<MappedType, ModelType>(DataModelProvider<ModelType> dataModelProvider) where ModelType : class
        {
            BusinessModelProvider<MappedType, ModelType> dataProvider = this.CreateSubclassDataProvider<BusinessModelProvider<MappedType, ModelType>>(dataModelProvider);

            return dataProvider;
        }

        protected virtual T CreateSubclassDataProvider<T>(IDataModelProvider dataModelProvider) where T : class
        {
            Type dataProviderType = ReflectionHelper.GetSingleSubclass<T>(this.DataProviderTypes);
            if (dataProviderType == null)
            {
                throw new ArgumentException("无法获取DataProvider类型");
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
