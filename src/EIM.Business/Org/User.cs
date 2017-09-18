using EIM.Cache;
using EIM.Exceptions.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.Org
{
    public class User : UserInfo, IIdCodeGuidProvider, ICacheRefreshable<User>
    {
        public User(UserInfo userInfo)
        {
            this.SetInfo(userInfo);
        }

        public User Clone()
        {
            User clone = new User(this);

            return clone;
        }

        public User Change(UserChangeInfo changeInfo)
        {
            User snapshot = this.Clone();
            this.SetInfo(changeInfo);

            return snapshot;
        }

        public User Refresh(User cacheInfo)
        {
            User snapshot = this.Clone();
            this.SetInfo(cacheInfo);

            return snapshot;
        }

        protected virtual void SetInfo(UserBaseInfo info)
        {
            ObjectMapperHelper.Map(this, info);
        }
    }
}
