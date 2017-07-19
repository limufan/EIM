using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIM.Data;
using EIM.Data.Org;
using EIM.Business;
using EIM.Business.Org;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EIM.Core;

namespace EIM.UnitTest.Other
{
    [TestClass]

    public class BusinessModelProviderTests: TestBase
    {


        [TestMethod]
        public void Insert()
        {
            UserModel userModel = new UserModel { Guid = Guid.NewGuid().ToString() };

            using (EIMDbContext dbContext = new EIMDbContext())
            {
                EFDataModelProvider<UserModel> dataProvider = new EFDataModelProvider<UserModel>(dbContext);
                dataProvider.Insert(userModel);
            }

            BusinessManager businessManager = new BusinessManager();
            EFDataModelProviderFactory efDataModelProviderFactory = new EFDataModelProviderFactory();

            BusinessModelProviderFactory businessModelProviderFactory = new BusinessModelProviderFactory(businessManager, efDataModelProviderFactory);
            using (BusinessModelProvider<User, UserModel> businessModelProvider = businessModelProviderFactory.CreateDataProvider<User, UserModel>())
            {
                User user = businessModelProvider.GetById(userModel.Id);
                Assert.IsNotNull(user);
                Assert.AreEqual(userModel.Guid, user.Guid);
            }

        }

        [TestMethod]
        public void CreateDataProvider ()
        {
            BusinessModelProvider<User, UserModel> businessModelProvider 
                = this.BusinessManager.BusinessModelProviderFactory.CreateDataProvider<User, UserModel>();
            Assert.IsNotNull(businessModelProvider);
        }
    }
}
