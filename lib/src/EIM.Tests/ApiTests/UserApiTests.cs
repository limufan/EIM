using EIM.Api.Org;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Tests.ApiTests
{
    public class UserApiTests: TestBase
    {
        [Test]
        public void Create()
        {
            UserApi userApi = new UserApi();
            UserCreateModel createModel = this.RandomDataFiller.GetValue<UserCreateModel>();

            userApi.Create(createModel);
        }
    }
}
