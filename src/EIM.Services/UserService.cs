using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIM.Api.Org;
using EIM.Core;
using EIM.Business.Org;

namespace EIM.Services
{
    public class UserService: EIMService
    {
        public UserService()
        {
            
        }

        public UserDetailsModel Create(UserCreateModel createModel)
        {
            UserCreateInfo createInfo = this.MapperFactory.Map<UserCreateInfo, UserCreateModel>(createModel);
            User user = this.BusinessManager.UserManager.Create(createInfo);

            return this.MapperFactory.Map<UserDetailsModel, User>(user);
        }
    }
}
