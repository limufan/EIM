using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIM.Business;
using EIM.Core;
using System.Reflection;

namespace EIM.Data.DataModelProviders
{
    public abstract class DataModelProviderFactory
    {
        public DataModelProviderFactory(params Assembly[] assemblys)
        {
            this.DataProviders = new List<IDataModelProvider>();
            List<Type> dataProviderTypes = new List<Type>();
            if (assemblys != null && assemblys.Length > 0)
            {
                dataProviderTypes.AddRange(ReflectionHelper.GetSubclass<IDataModelProvider>(assemblys));
            }
            dataProviderTypes.AddRange(ReflectionHelper.GetSubclass<IDataModelProvider>(typeof(DataModelProviderFactory).Assembly));
            this.DataProviderTypes = dataProviderTypes.ToArray();
        }

        public List<IDataModelProvider> DataProviders { private set; get; }

        public Type[] DataProviderTypes { set; get; }

        public abstract T CreateDataProviderByType<T>() where T : IDataModelProvider;

        public abstract object CreateDataProviderByType(Type type);

        public virtual DataModelProvider<ModelType> CreateDataProvider<ModelType>() where ModelType : class
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

            T dataProvider = this.CreateDataProviderByType(dataProviderType) as T;

            if (dataProvider == null)
            {
                throw new ArgumentException("无法创建DataProvider");
            }

            return dataProvider;
        }
    }
}
