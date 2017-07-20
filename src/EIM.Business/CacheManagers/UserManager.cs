using EIM.Business.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.CacheManagers
{
    public class UserManager : CacheManager<User>
    {
        public UserManager()
        {
            this.ByIdIndex = new ByIdCacheIndex<User>(this);
            this.CacheIndexes.Add(this.ByIdIndex);
        }

        public ByIdCacheIndex<User> ByIdIndex { set; get; }

        public virtual User GetById(int id)
        {
            return this.ByIdIndex.GetById(id);
        }

        public int GetByIdCacheCount()
        {
            return this.ByIdIndex.GetByIdCacheCount();
        }
    }
}
