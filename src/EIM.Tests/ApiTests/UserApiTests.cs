using EIM.Api;
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
            UserCreateModel createModel = this.RandomDataFiller.GetValue<UserCreateModel>();

            ApiManager.Instance.UserApi.Create(createModel);
        }
    }
}
