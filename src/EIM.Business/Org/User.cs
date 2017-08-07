using EIM.Cache;
using EIM.Exceptions.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.Org
{
    public class User : UserInfo, IIdCodeGuidProvider, ICache<User>
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

        public void Refresh(User cacheInfo)
        {
            this.SetInfo(cacheInfo);
        }

        protected virtual void SetInfo(UserBaseInfo info)
        {
            ObjectMapperHelper.Map(this, info);
        }
    }
}
