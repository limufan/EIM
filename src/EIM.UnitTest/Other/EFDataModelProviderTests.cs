using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIM.Data;
using EIM.Data.Org;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EIM.UnitTest.Other
{
    [TestClass]
    public class EFDataModelProviderTests : TestBase
    {
        [TestMethod]
        public void Insert()
        {
            using (EIMDbContext dbContext = new EIMDbContext())
            {
                EFDataModelProvider<UserModel> dataProvider = new EFDataModelProvider<UserModel>(dbContext);
                dataProvider.Insert(new UserModel { Guid = Guid.NewGuid().ToString() });
            }
                
        }

        [TestMethod]
        public void FacotryTest()
        {
            EFDataModelProviderFactory factroy = new EFDataModelProviderFactory();
            DataModelProvider<UserModel> userModelProvider = factroy.CreateDataProviderByModelType<UserModel>();

            Assert.IsNotNull(userModelProvider);
        }
    }
}
