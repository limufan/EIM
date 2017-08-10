using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIM.Data.DataModelProviders
{
    public class DapperDataModelProvider<ModelType> : EFDataModelProvider<ModelType>
        where ModelType : class
    {
        public DapperDataModelProvider(EIMDbContext dbContext)
            :base(dbContext)
        {
            TableAttribute table = ReflectionHelper.GetAttribute<TableAttribute>(typeof(ModelType));
            if(table != null)
            {
                this.TableName = table.Name;
            }
        }

        public string TableName { set; get; }

        public override List<ModelType> GetModels()
        {
            string sql = string.Format("select *from {0};", this.TableName);

            if(this.DbContext.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                this.DbContext.Database.Connection.Open();
            }

            return this.DbContext.Database.
                Connection.
                Query<ModelType>(sql)
                .ToList();
        }
    }
}
