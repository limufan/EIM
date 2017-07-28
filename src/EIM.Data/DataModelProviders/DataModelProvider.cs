using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;

namespace EIM.Data
{
    public interface IDataModelProvider
    {

    }

    public abstract class DataModelProvider<ModelType> : IDisposable, IDataModelProvider
        where ModelType : class
    {

        public abstract void Insert(ModelType model);

        public abstract void Update(ModelType model);

        public abstract void Delete(ModelType model);

        public abstract void Delete(Expression<Func<ModelType, bool>> expression);

        public abstract void Refresh(ModelType model);

        public abstract List<ModelType> GetTopModels(int count);

        public abstract List<ModelType> GetModels();

        public abstract List<ModelType> SelectModels(Expression<Func<ModelType, bool>> expression);

        public abstract ModelType SelectFirst(Expression<Func<ModelType, bool>> expression);

        public abstract ModelType SelectById(object id);

        public abstract int Count();

        public abstract bool Exist(ModelType model);

        public abstract IDbTransaction BeginTransaction();

        public virtual void Dispose()
        {
            this.Close();
        }

        public abstract void Close();
    }
}
