using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIM.Business;
using EIM.Core;
using EIM.Data.Org;

namespace EIM.Data
{
    public class EIMDbContext : DbContext
    {
        public EIMDbContext()
            :base("EIM")
        {
            
        }

        public DbSet<UserDataModel> Users { get; set; }

        public DbSet<DepartmentDataModel> Departments { get; set; }

        public List<object> DbSetList { set; get; }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            DbSet<T> dbset = null;
            if (ReflectionHelper.TypeEqual<T, UserDataModel>())
            {
                dbset = this.Users as DbSet<T>;
            }
            else if (ReflectionHelper.TypeEqual<T, DepartmentDataModel>())
            {
                dbset = this.Departments as DbSet<T>;
            }
            return dbset;
        }
        
    }
}
