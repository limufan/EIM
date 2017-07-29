using EIM.Business;
using EIM.Business.Org;
using EIM.Core;
using EIM.Data.Org;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Tests.FactoryTests
{
    public class BusinessModelMapperFactoryTests
    {

        [Test]
        public void CreateMapper()
        {
            CacheContainer cacheContainer = new CacheContainer();
            BusinessModelMapperFactory factory = new BusinessModelMapperFactory(cacheContainer, typeof(BusinessModelMapperFactoryTests).Assembly);

            TBusinessModelMapper<User, UserDataModel> userMapper = factory.Create<User, UserDataModel>();
            Assert.NotNull(userMapper);

            TBusinessModelMapper<TestBusinessModel, TestDataModel> testMapper = factory.Create<TestBusinessModel, TestDataModel>();
            Assert.NotNull(testMapper);
            Assert.True(testMapper is TestMapper);
        }


        public class TestMapper: TBusinessModelMapper<TestBusinessModel, TestDataModel>
        {
            public TestMapper(CacheContainer cacheContainer)
                : base(cacheContainer)
            {

            }
        }
    }
}
