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
    public interface IMappedDataProvider
    {

    }

    public class MappedDataProvider<MappedType, ModelType> : DataProvider<ModelType>, IMappedDataProvider
        where ModelType : class
    {
        public MappedDataProvider(DataModelMapperFactory mapperFactory, EIMDbContext dbContext)
            : base(dbContext)
        {
            DataModelMapper<MappedType, ModelType> mapper = mapperFactory.CreateMapper<MappedType, ModelType>();
            this.Mapper = mapper;
        }

        public DataModelMapper<MappedType, ModelType> Mapper { set; get; }

        internal virtual void OnInserted(ModelType model)
        {
            
        }

        internal virtual void OnUpdated(ModelType model)
        {
            
        }

        internal virtual void OnDeleted(ModelType model)
        {
            
        }

        public virtual MappedType GetById(object id)
        {
            ModelType model = this.SelectById(id);
            return this.Map(model);
        }

        public virtual List<MappedType> GetTop(int count)
        {
            IList<ModelType> models = this.GetTopModels(count);

            return models.Select(m => this.Mapper.Map(m)).ToList();
        }

        public virtual MappedType GetFirst(Expression<Func<ModelType, bool>> expression)
        {
            ModelType model = this.SelectFirst(expression);
            return this.Map(model);
        }

        public virtual List<MappedType> Where(Expression<Func<ModelType, bool>> expression)
        {
            List<ModelType> models = this.SelectModels(expression);
            return models.Select(m => this.Map(m)).ToList();
        }

        public virtual List<MappedType> Where(Expression<Func<ModelType, bool>> expression, int count)
        {
            List<ModelType> models = this.SelectModels(expression);
            return models.Select(m => this.Map(m)).ToList();
        }

        public virtual MappedType Map(ModelType model)
        {
            return this.Mapper.Map(model);
        }
    }
}
