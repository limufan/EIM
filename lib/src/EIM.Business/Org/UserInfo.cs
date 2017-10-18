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


        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public UserGender Gender { get; set; }

        public UserRole Role { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus Status { get; set; }

        public Position MainPosition { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后一次登录IP
        /// </summary>
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public Department Department { get; set; }
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
