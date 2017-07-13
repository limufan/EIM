using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIM.Business;
using EIM.Core;


namespace EIM.Data
{


    public class EFDataModelProviderFactory : DataModelProviderFactory
    {
        public EFDataModelProviderFactory()
        {

        }

        public EIMDbContext CreateDbContext(string code)
        {
            EIMDbContext context = new EIMDbContext();
            return context;
        }

        public override T CreateDataProvider<T>()
        {
            EIMDbContext context = this.CreateDbContext(typeof(T).Name);

            return this.CreateDataProvider<T>(context);
        }

        public override object CreateDataProvider(Type type)
        {
            EIMDbContext context = this.CreateDbContext(type.Name);

            return this.CreateDataProvider(type, context);
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
    }
}
