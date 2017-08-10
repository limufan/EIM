using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data.DataModelProviders
{
    public class EFDataModelProvider<ModelType> : DataModelProvider<ModelType>
        where ModelType : class
    {
        public EFDataModelProvider(EIMDbContext dbContext)
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

        public override void Insert(ModelType model)
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

        public override void Update(ModelType model)
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

        public override void Delete(ModelType model)
        {
            if (model == null)
            {
                return;
            }

            this.DbSet.Remove(model);
            this.DbContext.SaveChanges();
        }

        public override void Delete(Expression<Func<ModelType, bool>> expression)
        {
            List<ModelType> models = this.DbSet.Where(expression).ToList();
            foreach (ModelType model in models)
            {
                this.DbSet.Remove(model);
            }
            this.DbContext.SaveChanges();
        }

        public override void Refresh(ModelType model)
        {
            this.DbContext.Entry<ModelType>(model).Reload();
        }

        public override List<ModelType> GetTopModels(int count)
        {
            List<ModelType> models =
                this.DbSet.Take(count)
                    .ToList();

            return models;
        }

        public override List<ModelType> GetModels()
        {
            return this.DbSet
                    .ToList();
        }

        public override List<ModelType> SelectModels(Expression<Func<ModelType, bool>> expression)
        {
            return this.DbSet.Where(expression)
                    .ToList();
        }

        public override ModelType SelectFirst(Expression<Func<ModelType, bool>> expression)
        {
            return this.DbSet.First(expression);
        }

        public override ModelType SelectById(object id)
        {
            return this.DbSet.Find(id);
        }

        public override int Count()
        {
            return this.DbSet.Count();
        }

        public override bool Exist(ModelType model)
        {
            return this.DbSet.Contains(model);
        }

        public override void Close()
        {
            this.DbContext.Dispose();
        }

        public override IDbTransaction BeginTransaction()
        {
            DbContextTransaction transaction = this.DbContext.Database.BeginTransaction();
            return new EFDbTransaction(transaction);
        }
    }
}
