using EIM.Business;
using EIM.Business.Org;
using EIM.Core;
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
    public class BusinessModelProviderFactoryTests
    {
        [Test]
        public void CreateProvider()
        {
            CacheContainer cacheContainer = new CacheContainer();
            DataModelProviderFactory dataProviderFactory = new TestEFDataModelProviderFactory();
            BusinessModelProviderFactory factory = new BusinessModelProviderFactory(cacheContainer, dataProviderFactory, typeof(BusinessModelProviderFactoryTests).Assembly);

            BusinessModelProvider<User, UserDataModel> userProvider = factory.CreateProvider<User, UserDataModel>();
            Assert.NotNull(userProvider);


            BusinessModelProvider<TestBusinessModel, TestDataModel> testProvider = factory.CreateProvider<TestBusinessModel, TestDataModel>();
            Assert.NotNull(testProvider);
            Assert.True(testProvider is TestBusinessModelProvider);
        }

        public class TestBusinessModelProvider: BusinessModelProvider<TestBusinessModel, TestDataModel>
        {
            public TestBusinessModelProvider(DataModelMapperFactory mapperFactory, DataModelProvider<TestDataModel> dataProvider)
                :base(mapperFactory, dataProvider)
            {
                
            }
        }
    }
}
