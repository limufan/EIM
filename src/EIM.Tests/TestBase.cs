﻿using EIM.Business;
using EIM.Business.Org;
using EIM.Core;
using EIM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Tests
{
    public class TestBase
    {
        static TestBase()
        {
            MigrationsConfiguration.SetInitializer();
        }

        public TestBase()
        {
            this.RandomDataFiller = new RandomDataFiller();
            this.ObjectEqualAsserter = new ObjectEqualAsserter();
            this.BusinessManager = new BusinessManager();
        }

        public BusinessManager BusinessManager { set; get; }

        public RandomDataFiller RandomDataFiller { set; get; }

        public ObjectEqualAsserter ObjectEqualAsserter { set; get; }

        public User CreateUser()
        {
            UserCreateInfo createInfo = this.RandomDataFiller.GetValue<UserCreateInfo>();
            User user = this.BusinessManager.UserManager.Create(createInfo);

            return user;
        }

        public User Admin { set; get; }
    }
}
