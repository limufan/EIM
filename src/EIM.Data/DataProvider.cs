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
    public interface IDataProvider
    {

    }

    public class DataProvider<ModelType> : IDisposable, IDataProvider
        where ModelType : class
    {
        public DataProvider(EIMDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext");
            }
            this.DbContext = dbContext;
            this.DbSet = dbContext.GetDbSet<ModelType>();
            if (this.DbSet == null)
            {
                throw new ArgumentNullException("this.DbSet");
            }
        }

        internal DbContext DbContext { set; get; }

        internal DbSet<ModelType> DbSet { set; get; }

        public virtual void Insert(ModelType model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }
                
                this.DbSet.Add(model);
                this.DbContext.SaveChanges();
            }
            catch
            {
                EIMLog.Logger.Error("DataProvider Inser Error:" + JsonConvertHelper.SerializeObject(model));
                throw;
            }
        }

        public virtual void Update(ModelType model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException("model");
                }
                
                this.DbContext.SaveChanges();
            }
            catch
            {
                EIMLog.Logger.Error("DataProvider Update Error:" + JsonConvertHelper.SerializeObject(model));
                throw;
            }
        }

        public virtual void Delete(ModelType model)
        {
            if (model == null)
            {
                return;
            }

            this.DbSet.Remove(model);
            this.DbContext.SaveChanges();
        }

        public virtual void Delete(Expression<Func<ModelType, bool>> expression)
        {
            List<ModelType> models = this.DbSet.Where(expression).ToList();
            foreach(ModelType model in models)
            {
                this.DbSet.Remove(model);
            }
            this.DbContext.SaveChanges();
        }

        public virtual List<ModelType> GetTopModels(int count)
        {
            List<ModelType> models =
                this.DbSet.Take(count)
                    .ToList();

            return models;
        }

        public virtual List<ModelType> GetModels()
        {
            return this.DbSet
                    .ToList();
        }

        public virtual List<ModelType> SelectModels(Expression<Func<ModelType, bool>> expression)
        {
            return this.DbSet.Where(expression)
                    .ToList();
        }

        public virtual ModelType SelectFirst(Expression<Func<ModelType, bool>> expression)
        {
            return this.DbSet.First(expression);
        }

        public virtual ModelType SelectById(object id)
        {
            return this.DbSet.Find(id);
        }

        public virtual int Count()
        {
            return this.DbSet.Count();
        }

        public virtual bool Exist(ModelType model)
        {
            return this.DbSet.Contains(model);
        }

        public void Dispose()
        {
            this.Close();
        }

        public void Close()
        {
            this.DbContext.Dispose();
        }
    }
}
