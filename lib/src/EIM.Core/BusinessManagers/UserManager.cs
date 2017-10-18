using EIM.Business;
using EIM.Business.Org;
using EIM.Core.Events;
using EIM.Data;
using EIM.Data.DataModelProviders;
using EIM.Data.Org;
using EIM.Exceptions.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.BusinessManagers
{
    public class UserManager
    {
        public UserManager(BusinessManager businessManager)
        {
            this.MapperFactory = businessManager.MapperFactory;
            this.DataModelProviderFactory = new EFDataModelProviderFactory();
            this.CacheContainer = businessManager.CacheContainer;
            this.UserEvents = businessManager.EventManager.UserEvents;
        }

        public BusinessManager BusinessManager { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }

        public DataModelMapperFactory MapperFactory { set; get; }

        public EIMCacheContainer CacheContainer { set; get; }

        public UserEvents UserEvents { set; get; }

        public User Create(UserCreateInfo createInfo)
        {
            this.UserEvents.OnCreating(createInfo);
            UserDataModel model = this.MapperFactory.Map<UserDataModel, UserCreateInfo>(createInfo);

            using (DataModelProvider<UserDataModel> dataModelProvider = this.DataModelProviderFactory.CreateDataProvider<UserDataModel>())
            {
                dataModelProvider.Insert(model);
            }

            User user = this.MapperFactory.Map<User, UserDataModel>(model);
            this.UserEvents.OnCreated(user, createInfo);

            return user;
        }

        public void Change(UserChangeInfo changeInfo)
        {
            this.UserEvents.OnChanging(changeInfo);

            using (DataModelProvider<UserDataModel> dataModelProvider = this.DataModelProviderFactory.CreateDataProvider<UserDataModel>())
            {
                UserDataModel model = dataModelProvider.SelectById(changeInfo.ChangeUser.Id);
                this.MapperFactory.Map<UserDataModel, UserChangeInfo>(model, changeInfo);
                dataModelProvider.Update(model);

                this.UserEvents.OnChanged(changeInfo);
            }
        }

        public void Delete(User user, OperationInfo opInfo)
        {
            using (DataModelProvider<UserDataModel> dataModelProvider = this.DataModelProviderFactory.CreateDataProvider<UserDataModel>())
            {
                UserDataModel model = dataModelProvider.SelectById(user.Id);
                dataModelProvider.Update(model);

                this.UserEvents.OnDeleted(user, opInfo);
            }
        }

        public virtual void ChangePassword(User changeUser, string oldPassword, string newPassword)
        {
            string encodedOldPassword = Cryptography.MD5Encode(oldPassword);
            if (changeUser.Password != encodedOldPassword)
            {
                throw new OldPasswordWrongException();
            }
            string encodedNewPassword = Cryptography.MD5Encode(newPassword);
            UserChangeInfo changeInfo = new UserChangeInfo(changeUser);
            changeInfo.Password = encodedNewPassword;

            this.Change(changeInfo);
        }

        public virtual void ResetPassword(User user, string newPassword, OperationInfo opInfo)
        {
            string encodedNewPassword = Cryptography.MD5Encode(newPassword);
            UserChangeInfo changeInfo = new UserChangeInfo(user);
            changeInfo.Password = encodedNewPassword;

            this.Change(changeInfo);

            this.UserEvents.OnPasswordReseted(user, opInfo);
        }

        public virtual void ImportPassword(string password, User user, OperationInfo opInfo)
        {
            UserChangeInfo changeInfo = new UserChangeInfo(user);
            changeInfo.Password = password;

            this.Change(changeInfo);
        }

        public virtual void Lock(User lockUser, OperationInfo opInfo)
        {
            UserStatus status = UserStatus.Lock;
            UserChangeInfo changeInfo = new UserChangeInfo(lockUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            this.UserEvents.OnLocked(lockUser, opInfo);
        }

        public virtual void Unlock(User unlockUser, OperationInfo opInfo)
        {
            UserStatus status = UserStatus.Normal;
            UserChangeInfo changeInfo = new UserChangeInfo(unlockUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            this.UserEvents.OnActivated(unlockUser, opInfo);
        }

        public virtual void Logoff(User logoffUser, OperationInfo opInfo)
        {
            UserStatus status = UserStatus.Logoff;
            UserChangeInfo changeInfo = new UserChangeInfo(logoffUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            this.UserEvents.OnLogoff(logoffUser, opInfo);
        }

        public virtual void Activate(User activateUser, OperationInfo opInfo)
        {
            UserStatus status = UserStatus.Normal;
            UserChangeInfo changeInfo = new UserChangeInfo(activateUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            this.UserEvents.OnActivated(activateUser, opInfo);
        }

        public User GetById(int id)
        {
            return this.CacheContainer.UserCacheManager.GetById(id);
        }

        public User GetByCode(string code)
        {
            return this.CacheContainer.UserCacheManager.GetByCode(code);
        }

        public User GetByGuid(string guid)
        {
            return this.CacheContainer.UserCacheManager.GetByGuid(guid);
        }

        public User GetByAccount(string account)
        {
            return this.CacheContainer.UserCacheManager.GetByAccount(account);
        }
    }
}
