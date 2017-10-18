using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace EIM.Api.Org
{
    public class UserApi : BaseApi
    {
        public UserApi(JsonServiceClient client, ApiManagerBase apiManager) : base(client, apiManager)
        {

        }

        public UserDetailsModel Create(UserCreateModel createModel)
        {
            return this.Post<UserDetailsModel>(createModel);
        }
    }
}
