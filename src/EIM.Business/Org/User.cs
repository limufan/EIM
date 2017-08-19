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

        public void Change(UserChangeInfo changeInfo)
        {
            this.SetInfo(changeInfo);
        }

        public User Clone()
        {
            return new User(this);
        }

        public User Refresh(User cacheInfo)
        {
            User snapshot = new User(this);
            this.SetInfo(cacheInfo);

            return snapshot;
        }

        protected virtual void SetInfo(UserBaseInfo info)
        {
            ObjectMapperHelper.Map(this, info);
        }
    }
}
