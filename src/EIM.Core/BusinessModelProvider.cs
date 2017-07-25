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
using EIM.Data;
using EIM.Exceptions;
using EIM.Business;

namespace EIM.Core
{
    public interface IBusinessModelProvider
    {

    }

    public class BusinessModelProvider<BusinessType, ModelType> : IDisposable, IBusinessModelProvider
        where BusinessType : IKeyProvider
        where ModelType : class, IKeyProvider
    {
        public BusinessModelProvider(DataModelMapperFactory mapperFactory, DataModelProvider<ModelType> dataProvider)
        {
            DataModelMapper<BusinessType, ModelType> mapper = mapperFactory.CreateMapper<BusinessType, ModelType>();
            this.Mapper = mapper;
            this.DataProvider = dataProvider;
        }

        public DataModelMapper<BusinessType, ModelType> Mapper { set; get; }

        public DataModelProvider<ModelType> DataProvider { set; get; }

        public virtual BusinessType Create(object createInfo)
        {
            ModelType dataModel = this.Mapper.Map(createInfo);
            this.DataProvider.Insert(dataModel);

            BusinessType businessObject = this.GetById(dataModel.GetKey());
            if (businessObject == null)
            {
                throw new EIMException("创建对象失败:" +  typeof(BusinessType).Name);
            }

            return businessObject;
        }

        public virtual void Change(IChangeInfo changeInfo)
        {
            ModelType dataModel = this.DataProvider.SelectById(changeInfo.GetKey());
            this.Mapper.Map(dataModel, changeInfo);

            this.DataProvider.Update(dataModel);
        }

        public virtual void Delete(BusinessType businessObject)
        {
            ModelType dataModel = this.DataProvider.SelectById(businessObject.GetKey());

            this.DataProvider.Delete(dataModel);
        }

        public virtual BusinessType GetById(object id)
        {
            ModelType model = this.DataProvider.SelectById(id);
            return this.Map(model);
        }

        public virtual List<BusinessType> GetTop(int count)
        {
            IList<ModelType> models = this.DataProvider.GetTopModels(count);

            return models.Select(m => this.Mapper.Map(m)).ToList();
        }

        public virtual BusinessType GetFirst(Expression<Func<ModelType, bool>> expression)
        {
            ModelType model = this.DataProvider.SelectFirst(expression);
            return this.Map(model);
        }

        public virtual List<BusinessType> Where(Expression<Func<ModelType, bool>> expression)
        {
            List<ModelType> models = this.DataProvider.SelectModels(expression);
            return models.Select(m => this.Map(m)).ToList();
        }

        public virtual List<BusinessType> Where(Expression<Func<ModelType, bool>> expression, int count)
        {
            List<ModelType> models = this.DataProvider.SelectModels(expression);
            return models.Select(m => this.Map(m)).ToList();
        }

        protected virtual BusinessType Map(ModelType model)
        {
            return this.Mapper.Map(model);
        }

        public virtual void Dispose()
        {
            this.Close();
        }

        public virtual void Close()
        {
            this.DataProvider.Close();
        }
    }
}
