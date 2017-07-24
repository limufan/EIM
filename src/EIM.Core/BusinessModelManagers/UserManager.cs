using EIM.Business.Org;
using EIM.Data;
using EIM.Data.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.BusinessModelManagers
{
    public class UserManager
    {
        public UserManager(BusinessManager businessManager)
        {
            this.BusinessManager = businessManager;
        }

        public BusinessManager BusinessManager { set; get; }

        public User Create(UserCreateInfo createInfo)
        {
            using (BusinessModelProvider<User, UserDataModel> businessModelProvider = 
                this.BusinessManager.BusinessModelProviderFactory.CreateDataProvider<User, UserDataModel>())
            {
                UserDataModel dataModel = businessModelProvider.Mapper.Map(createInfo);

                businessModelProvider.DataProvider.Insert(dataModel);
                return businessModelProvider.GetById(dataModel.Id);
            }
        }

        public void Chnage(UserChangeInfo changeInfo)
        {
            using (BusinessModelProvider<User, UserDataModel> businessModelProvider =
                this.BusinessManager.BusinessModelProviderFactory.CreateDataProvider<User, UserDataModel>())
            {
                UserDataModel dataModel = businessModelProvider.DataProvider.SelectById(changeInfo.ChangeUser.Id);
                businessModelProvider.Mapper.Map(dataModel, changeInfo);

                businessModelProvider.DataProvider.Update(dataModel);
            }
        }

        public void Delete(User user)
        {
            using (BusinessModelProvider<User, UserDataModel> businessModelProvider =
                this.BusinessManager.BusinessModelProviderFactory.CreateDataProvider<User, UserDataModel>())
            {
                UserDataModel dataModel = businessModelProvider.DataProvider.SelectById(user.Id);

                businessModelProvider.DataProvider.Delete(dataModel);
            }
        }
    }
}
