using EIM.Data;
using EIM.Data.DataModelProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Tests
{
    public class TestEFDataModelProviderFactory: EFDataModelProviderFactory
    {
        public TestEFDataModelProviderFactory() : base(typeof(TestEFDataModelProviderFactory).Assembly)
        {

        }

        public override EIMDbContext CreateDbContext(string code)
        {
            TestEIMDbContext context = new TestEIMDbContext();
            return context;
        }
    }
}
