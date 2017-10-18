using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Api.Org
{
    public class UserApi: RpcClient
    {
        public void Create(UserCreateModel createModel)
        {
            this.Reqeust<UserDetailsModel>(createModel);
        }
    }
}
