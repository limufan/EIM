using EIM.Business.Org;
using EIM.Cache.CacheIndexes;
using EIM.Cache.CacheManagers;
using EIM.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core
{
    public class UserCacheManager : ByIdCodeGuidCacheManager<User>
    {
        public UserCacheManager(EventManager eventManager)
        {
            this.UserEvents = eventManager.UserEvents;

            this.UserEvents.Created += UserEventManager_Created;
            this.UserEvents.Changed += UserEventManager_Changed;
            this.UserEvents.Deleted += UserEventManager_Deleted;
        }

        public UserEvents UserEvents { set; get; }

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

        private void UserEventManager_Deleted(User user, Business.OperationInfo args)
        {
            this.Remove(user);
        }

        private void UserEventManager_Changed(User user, UserChangeInfo changeInfo)
        {
            User snapshot = changeInfo.ChangeUser.Change(changeInfo);
            this.Change(changeInfo.ChangeUser, snapshot);
        }

        private void UserEventManager_Created(User user, UserCreateInfo args)
        {
            this.Add(user);
        }
    }
}
