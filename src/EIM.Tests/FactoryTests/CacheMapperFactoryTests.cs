using EIM.Business;
using EIM.Business.Org;
using EIM.Cache;
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
    public class CacheMapperFactoryTests
    {

        [Test]
        public void CreateMapper()
        {
            CacheContainer cacheContainer = new CacheContainer();
            CacheMapperFactory factory = new CacheMapperFactory(cacheContainer, typeof(CacheMapperFactoryTests).Assembly);

            TCacheMapper<User, UserDataModel> userMapper = factory.Create<User, UserDataModel>();
            Assert.NotNull(userMapper);

            TCacheMapper<TestBusinessModel, TestDataModel> testMapper = factory.Create<TestBusinessModel, TestDataModel>();
            Assert.NotNull(testMapper);
            Assert.True(testMapper is TestMapper);
        }


        public class TestMapper: TCacheMapper<TestBusinessModel, TestDataModel>
        {
            public TestMapper(CacheContainer cacheContainer)
                : base(cacheContainer)
            {

            }
        }
    }
}
