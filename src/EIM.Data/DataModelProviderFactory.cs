using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIM.Business;
using EIM.Core;

namespace EIM.Data
{
    public abstract class DataModelProviderFactory
    {
        public DataModelProviderFactory()
        {
            this.DataProviders = new List<IDataModelProvider>();
            this.DataProviderTypes = ReflectionHelper.GetSubclass<IDataModelProvider>(this.GetType().Assembly);
        }

        public List<IDataModelProvider> DataProviders { private set; get; }

        public Type[] DataProviderTypes { set; get; }

        public abstract T CreateDataProvider<T>() where T : IDataModelProvider;

        public abstract object CreateDataProvider(Type type);

        public virtual DataModelProvider<ModelType> CreateDataProviderByModelType<ModelType>() where ModelType : class
        {
            DataModelProvider<ModelType> dataProvider = this.CreateSubclassDataProvider<DataModelProvider<ModelType>>();

            return dataProvider;
        }

        protected virtual T CreateSubclassDataProvider<T>() where T : class
        {
            Type dataProviderType = ReflectionHelper.GetSingleSubclass<T>(this.DataProviderTypes);
            if (dataProviderType == null)
            {
                return null;
            }

            T dataProvider = this.CreateDataProvider(dataProviderType) as T;

            if (dataProvider == null)
            {
                throw new ArgumentException("无法创建DataProvider");
            }

            return dataProvider;
        }


    }
}
