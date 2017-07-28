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
            this.DbSetList = new List<object>();
            this.DbSetList.Add(this.Users);
            this.DbSetList.Add(this.Departments);
        }

        public DbSet<UserDataModel> Users { get; set; }

        public DbSet<DepartmentDataModel> Departments { get; set; }

        public List<object> DbSetList { set; get; }

        public DbSet<T> GetDbSet<T>() where T : class
        {
            foreach(object dbSet in this.DbSetList)
            {
                if(dbSet is DbSet<T>)
                {
                    return dbSet as DbSet<T>;
                }
            }
            return null;
        }

    }
}
