using EIM.Business;
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
            this.CacheContainer = businessManager.CacheContainer;
            this.BusinessModelProviderFactory = businessManager.BusinessModelProviderFactory;
        }

        public BusinessManager BusinessManager { set; get; }

        public BusinessModelProviderFactory BusinessModelProviderFactory { set; get; }

        public CacheContainer CacheContainer { set; get; }

        public event TEventHandler<User> Created;

        public event TEventHandler<UserChangeInfo> Changed;

        public event TEventHandler<User> Deleted;

        public User Create(UserCreateInfo createInfo)
        {
            using (BusinessModelProvider<User, UserDataModel> businessModelProvider = 
                this.BusinessModelProviderFactory.CreateProvider<User, UserDataModel>())
            {
                User user = businessModelProvider.Create(createInfo);

                this.OnCreated(user);

                return user;
            }
        }

        public void Chnage(UserChangeInfo changeInfo)
        {
            using (BusinessModelProvider<User, UserDataModel> businessModelProvider =
                this.BusinessModelProviderFactory.CreateProvider<User, UserDataModel>())
            {
                businessModelProvider.Change(changeInfo);

                this.OnChanged(changeInfo);
            }
        }

        public void Delete(User user)
        {
            using (BusinessModelProvider<User, UserDataModel> businessModelProvider =
                this.BusinessModelProviderFactory.CreateProvider<User, UserDataModel>())
            {
                businessModelProvider.Delete(user);

                this.OnDeleted(user);
            }
        }

        public void OnCreated(User user)
        {
            this.CacheContainer.UserCacheManager.Add(user);
            if(this.Created != null)
            {
                this.Created(user);
            }
        }

        public void OnChanged(UserChangeInfo changeInfo)
        {
            changeInfo.ChangeUser.Change(changeInfo);
            if(this.Changed != null)
            {
                this.Changed(changeInfo);
            }
        }

        public void OnDeleted(User user)
        {
            this.CacheContainer.UserCacheManager.Remove(user);
            if(this.Deleted != null)
            {
                this.Deleted(user);
            }
        }


    }
}
