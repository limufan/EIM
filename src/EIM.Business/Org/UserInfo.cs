using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.Org
{
    public class UserBaseInfo
    {
        public string Code { set; get; }

        public string Guid { set; get; }

        public string Account { set; get; }
    }

    public class UserInfo: UserBaseInfo
    {
        public int Id { set; get; }

    }

    public class UserCreateInfo: UserBaseInfo
    {


    }

    public class UserChangeInfo : UserBaseInfo
    {
        public UserChangeInfo(User user)
        {
            ObjectMapperHelper.Map(this, user);
            this.ChangeUser = user;
        }

        public User ChangeUser { set; get; }
    }
}
