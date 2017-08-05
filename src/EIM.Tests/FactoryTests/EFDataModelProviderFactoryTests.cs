using EIM.Data;
using EIM.Data.Org;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Tests.FactoryTests
{
    public class EFDataModelProviderFactoryTests
    {

        [Test]
        public void CreateDataProviderByModelType()
        {
            TestEFDataModelProviderFactory factroy = new TestEFDataModelProviderFactory();

            DataModelProvider<TestDataModel> testModelProvider = factroy.CreateDataProvider<TestDataModel>();
            Assert.IsNotNull(testModelProvider);
            Assert.True(testModelProvider is TestDataModelProvider);

            DataModelProvider<TestDataModel_1> testModel_1Provider = factroy.CreateDataProvider<TestDataModel_1>();
            Assert.IsNotNull(testModel_1Provider);
            Assert.True(testModel_1Provider is EFDataModelProvider<TestDataModel_1>);
        }

        public class TestDataModelProvider : EFDataModelProvider<TestDataModel>
        {
            public TestDataModelProvider(EIMDbContext dbContext) : base(dbContext)
            {

            }
        }
    }
}
