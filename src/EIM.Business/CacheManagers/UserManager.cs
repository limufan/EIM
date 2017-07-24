using EIM.Business.CacheIndexes;
using EIM.Business.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.CacheManagers
{
    public class UserManager : ByIdCodeGuidCacheManager<User>
    {
        public UserManager()
        {
            
        }

        protected KeyFuncCacheIndex<User, string> ByAccountCacheIndex { private set; get; }

        protected override List<CacheIndex<User>> CreateCacheIndexes()
        {
            this.ByAccountCacheIndex = new KeyFuncCacheIndex<User, string>(this, u => u.Account);

            List<CacheIndex<User>> cacheIndexes = base.CreateCacheIndexes();
            cacheIndexes.Add(this.ByAccountCacheIndex);

            return cacheIndexes;
        }

        public User GetByAccount(string account)
        {
            return this.ByAccountCacheIndex.GetByKey(account);
        }
    }
}
