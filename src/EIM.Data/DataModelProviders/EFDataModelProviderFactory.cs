﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIM.Business;
using EIM.Core;
using System.Reflection;

namespace EIM.Data.DataModelProviders
{


    public class EFDataModelProviderFactory : DataModelProviderFactory
    {
        public EFDataModelProviderFactory(params Assembly[] assemblys) : base(assemblys)
        {

        }

        public virtual EIMDbContext CreateDbContext(string code)
        {
            EIMDbContext context = new EIMDbContext();
            return context;
        }

        public override T CreateDataProviderByType<T>()
        {
            EIMDbContext context = this.CreateDbContext(typeof(T).Name);

            return this.CreateDataProvider<T>(context);
        }

        public override object CreateDataProviderByType(Type type)
        {
            EIMDbContext context = this.CreateDbContext(type.Name);

            return this.CreateDataProvider(type, context);
        }

        public override DataModelProvider<ModelType> CreateDataProvider<ModelType>()
        {
            DataModelProvider<ModelType> dataProvider = base.CreateDataProvider<ModelType>();
            if(dataProvider == null)
            {
                EIMDbContext context = this.CreateDbContext("CreateDataProviderByModelType");
                dataProvider = this.CreateDefaultDataProvider<ModelType>(context);
            }

            return dataProvider;
        }

        public virtual T CreateDataProvider<T>(EIMDbContext context) where T : IDataModelProvider
        {
            T dataProvder = (T)this.CreateDataProvider(typeof(T), context);

            return dataProvder;
        }

        protected virtual object CreateDataProvider(Type type, EIMDbContext context)
        {
            IDataModelProvider dataProvider = Activator.CreateInstance(type, context) as IDataModelProvider;

            return dataProvider;
        }

        protected virtual DataModelProvider<ModelType> CreateDefaultDataProvider<ModelType>(EIMDbContext context)
            where ModelType : class
        {
            return new EFDataModelProvider<ModelType>(context); ;
        }
    }
}
