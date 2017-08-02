using EIM.Exceptions.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.Org
{
    public class User : IIdCodeGuidProvider, IKeyProvider
    {
        public User(UserInfo userInfo)
        {
            this.SetInfo(userInfo);
        }

        public int Id { set; get; }

        public string Code { set; get; }

        public string Guid { set; get; }

        public string Account { set; get; }


        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual UserGender Gender { get; set; }

        public virtual UserRole Role { get; set; }

        /// <summary>
        /// 用户状态
        /// </summary>
        public virtual UserStatus Status { get; set; }

        public virtual Position MainPosition { get; set; }

        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public virtual DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后一次登录IP
        /// </summary>
        public virtual string LastLoginIp { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        public virtual event TEventHandler<User, UserChangeInfo> Changing;
        public event TEventHandler<User, UserChangeInfo> Changed;

        public void Change(UserChangeInfo changeInfo)
        {
            if(this.Changing != null)
            {
                this.Changing(this, changeInfo);
            }

            this.SetInfo(changeInfo);

            if(this.Changed != null)
            {
                this.Changed(this, changeInfo);
            }
        }

        protected virtual void SetInfo(UserBaseInfo info)
        {
            ObjectMapperHelper.Map(this, info);
        }

        public object GetKey()
        {
            return this.Id;
        }
    }
}
