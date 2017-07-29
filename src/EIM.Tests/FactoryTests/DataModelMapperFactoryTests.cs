using EIM.Business;
using EIM.Business.Org;
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
    public class DataModelMapperFactoryTests
    {
        [Test]
        public void CreateMapper()
        {
            CacheContainer cacheContainer = new CacheContainer();

            DataModelMapperFactory factory = new DataModelMapperFactory(cacheContainer, typeof(TestDataModelMapper).Assembly);

            DataModelMapper<User, UserDataModel> userMapper = factory.CreateMapper<User, UserDataModel>();
            Assert.NotNull(userMapper);

            DataModelMapper<TestDataModel, TestBusinessModel> testMapper = factory.CreateMapper<TestDataModel, TestBusinessModel>();
            Assert.NotNull(testMapper);
            Assert.True(testMapper is TestDataModelMapper);
        }

        public class TestDataModelMapper : DataModelMapper<TestDataModel, TestBusinessModel>
        {
            public TestDataModelMapper(CacheContainer cacheContainer)
                : base(cacheContainer)
            {

            }
        }
    }
}
