using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Api.Org
{
    public class UserBaseModel
    {
        public string Code { set; get; }

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
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    public class UserDetailsModel: UserBaseModel
    {
        public string Guid { set; get; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public UserStatus Status { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后一次登录IP
        /// </summary>
        public string LastLoginIp { get; set; }
    }

    public class UserCreateModel : UserBaseModel
    {

    }
}
