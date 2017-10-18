using EIM.Business;
using EIM.Business.Org;
using EIM.Core.BusinessManagers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Tests.CoreTests
{
    public class UserManagerTests: CoreTestsBase
    {

        public UserManagerTests()
        {
            this.UserManager = new UserManager(this.BusinessManager);
        }

        public UserManager UserManager { set; get; }

        [Test]
        public void Create()
        {
            UserCreateInfo createInfo = this.RandomDataFiller.GetValue<UserCreateInfo>();
            User user = this.UserManager.Create(createInfo);
            try
            {
                this.ObjectEqualAsserter.AssertEqual(createInfo, user);
            }
            catch
            {
                this.Delete(user);
            }
        }

        [Test]
        public void Change()
        {
            User user = this.CreateUser();
            try
            {
                UserChangeInfo changeInfo = new UserChangeInfo(user);
                User reloadUser = this.UserManager.GetById(user.Id);
                this.ObjectEqualAsserter.AssertEqual(changeInfo, reloadUser);
            }
            catch
            {
                this.Delete(user);
            }
        }
    }
}
