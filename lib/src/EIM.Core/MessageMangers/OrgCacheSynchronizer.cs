using EIM.Cache;
using EIM.MessageQueue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.MessageMangers
{
    public class OrgCacheSynchronizer
    {
        EIMCacheProviderManager _cacheProviderManager;
        UserEventQueue _eventQueue;

        public OrgCacheSynchronizer(EIMCacheProviderManager cacheProviderManager)
        {
            this._cacheProviderManager = cacheProviderManager;
            this._eventQueue = new UserEventQueue();
        }

        public void Sync()
        {
            this.BindUserMessage();
        }

        private void BindUserMessage()
        {
            this._eventQueue.BindCreatedMessage(userId => this.RefreshUserCache(userId));
            this._eventQueue.BindChangedMessage(userId => this.RefreshUserCache(userId));
            this._eventQueue.BindDeletedMessage(userId => this.RefreshUserCache(userId));
        }

        private void RefreshUserCache(int userId)
        {
            this._cacheProviderManager.UserCacheProvider.Refresh(userId);
        }
    }
}
