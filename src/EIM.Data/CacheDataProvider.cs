using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EIM.Core;

namespace EIM.Data
{
    public class CacheDataProvider<CacheType, ModelType> : DataProvider<ModelType>
        where ModelType : class
    {
        public CacheDataProvider(CacheManagerContainer cacheManagerContainer, EIMDbContext dbContext)
            : base(dbContext)
        {
            this.CacheManagerContainer = cacheManagerContainer;
            this.CacheMapper = DataModelCacheMapperFactory.CreateMapper<CacheType, ModelType>(cacheManagerContainer);
        }

        public DataModelCacheMapper<CacheType, ModelType> CacheMapper { set; get; }

        public CacheManagerContainer CacheManagerContainer { set; get; }

        internal virtual void OnInserted(ModelType model)
        {
            
        }

        internal virtual void OnUpdated(ModelType model)
        {
            
        }

        internal virtual void OnDeleted(ModelType model)
        {
            
        }

        public virtual CacheType GetById(object id)
        {
            ModelType model = this.SelectById(id);
            return this.CacheMapper.Map(model);
        }

        public virtual List<CacheType> GetTop(int count)
        {
            IList<ModelType> models = this.GetTopModels(count);

            return models.Select(m => this.CacheMapper.Map(m)).ToList();
        }

        public virtual CacheType GetFirst(Expression<Func<ModelType, bool>> expression)
        {
            ModelType model = this.SelectFirst(expression);
            return this.CacheMapper.Map(model);
        }

        public virtual List<CacheType> Where(Expression<Func<ModelType, bool>> expression)
        {
            List<ModelType> models = this.SelectModels(expression);
            return models.Select(m => this.CacheMapper.Map(m)).ToList();
        }

        public virtual List<CacheType> Where(Expression<Func<ModelType, bool>> expression, int count)
        {
            List<ModelType> models = this.SelectModels(expression);
            return models.Select(m => this.CacheMapper.Map(m)).ToList();
        }
    }
}
