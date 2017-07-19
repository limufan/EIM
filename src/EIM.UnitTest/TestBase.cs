using EIM.Business;
using EIM.Core;
using EIM.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.UnitTest
{
    public class TestBase
    {
        static TestBase()
        {
            MigrationsConfiguration.SetInitializer();
        }

        public TestBase()
        {
            this.BusinessManager = new BusinessManager();
        }

        public BusinessManager BusinessManager { set; get; }
    }
}
