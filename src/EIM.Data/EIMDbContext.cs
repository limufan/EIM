using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIM.Business;
using EIM.Core;

namespace EIM.Data
{
    public class EIMDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

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
