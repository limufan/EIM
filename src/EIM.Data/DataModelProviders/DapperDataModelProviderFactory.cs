using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Data.DataModelProviders
{
    public class DapperDataModelProviderFactory: EFDataModelProviderFactory
    {
        public DapperDataModelProviderFactory(params Assembly[] assemblys) : base(assemblys)
        {

        }

        protected override DataModelProvider<ModelType> CreateDefaultDataProvider<ModelType>(EIMDbContext context)
        {
            return new DapperDataModelProvider<ModelType>(context);
        }
    }
}
