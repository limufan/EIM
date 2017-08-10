using EIM.Business.Org;
using EIM.Cache.CacheIndexes;
using EIM.Cache.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core
{
    public class UserCacheManager : ByIdCodeGuidCacheManager<User>
    {
        public UserCacheManager()
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
