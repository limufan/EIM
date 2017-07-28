using EIM.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Tests
{
    public class TestEIMDbContext: EIMDbContext
    {
        public TestEIMDbContext()
        {
            this.DbSetList.Add(TestModels);
            this.DbSetList.Add(Test_1_Models);
        }

        public DbSet<TestDataModel> TestModels { get; set; }

        public DbSet<TestDataModel_1> Test_1_Models { get; set; }
    }
}
