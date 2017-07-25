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

        public void Change(UserChangeInfo changeInfo)
        {
            this.SetInfo(changeInfo);
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
