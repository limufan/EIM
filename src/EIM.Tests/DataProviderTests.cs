using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIM.Data;
using EIM.Data.Org;
using NUnit.Framework;

namespace EIM.Tests
{
    public class DataProviderTests
    {
        [Test]
        public void Insert()
        {
            MigrationsConfiguration.SetInitializer();
            using (EIMDbContext dbContext = new EIMDbContext())
            {
                EFDataModelProvider<UserModel> dataProvider = new EFDataModelProvider<UserModel>(dbContext);
                dataProvider.Insert(new UserModel { Guid = Guid.NewGuid().ToString() });
            }
                
        }
    }
}
