using EIM.Business;
using EIM.Business.Org;
using EIM.Data;
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
            this.BusinessManager = businessManager;
            this.CacheContainer = businessManager.CacheContainer;
            this.DataModelProviderFactory = businessManager.DataModelProviderFactory;
        }

        public BusinessManager BusinessManager { set; get; }

        public DataModelProviderFactory DataModelProviderFactory { set; get; }

        public EIMCacheContainer CacheContainer { set; get; }

        public event TEventHandler<UserCreateInfo> Creating;
        public event TEventHandler<User> Created;
        public virtual event TEventHandler<User, UserChangeInfo> Changing;
        public event TEventHandler<User, UserChangeInfo> Changed;
        public event TEventHandler<User, OperationInfo> Deleted;
        public virtual event TEventHandler<User, OperationInfo> Logoffed;
        public virtual event TEventHandler<User, OperationInfo> Locked;
        public virtual event TEventHandler<User, OperationInfo> Activated;
        public virtual event TEventHandler<User, OperationInfo> PasswordReseted;

        public User Create(UserCreateInfo createInfo)
        {
            this.OnCreating(createInfo);
            UserDataModel model = this.BusinessManager.DataModelMapperFactory.Map<UserDataModel, UserCreateInfo>(createInfo);

            using (DataModelProvider<UserDataModel> dataModelProvider = this.DataModelProviderFactory.CreateDataProvider<UserDataModel>())
            {
                dataModelProvider.Insert(model);
            }

            User user = this.BusinessManager.DataModelMapperFactory.Map<User, UserDataModel>(model);
            this.OnCreated(user);

            return user;
        }

        public void Change(UserChangeInfo changeInfo)
        {
            this.OnChanging(changeInfo);

            using (DataModelProvider<UserDataModel> dataModelProvider = this.DataModelProviderFactory.CreateDataProvider<UserDataModel>())
            {
                UserDataModel model = dataModelProvider.SelectById(changeInfo.ChangeUser.Id);
                this.BusinessManager.DataModelMapperFactory.Map<UserDataModel, UserChangeInfo>(model, changeInfo);
                dataModelProvider.Update(model);

                this.OnChanged(changeInfo);
            }
        }

        public void Delete(User user, OperationInfo opInfo)
        {
            using (DataModelProvider<UserDataModel> dataModelProvider = this.DataModelProviderFactory.CreateDataProvider<UserDataModel>())
            {
                UserDataModel model = dataModelProvider.SelectById(user.Id);
                dataModelProvider.Update(model);

                this.OnDeleted(user, opInfo);
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

            if (this.PasswordReseted != null)
            {
                this.PasswordReseted(user, opInfo);
            }
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

            if (this.Locked != null)
            {
                this.Locked(lockUser, opInfo);
            }
        }

        public virtual void Unlock(User unlockUser, OperationInfo opInfo)
        {
            UserStatus status = UserStatus.Normal;
            UserChangeInfo changeInfo = new UserChangeInfo(unlockUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            if (this.Activated != null)
            {

                this.Activated(unlockUser, opInfo);
            }
        }

        public virtual void Logoff(User logoffUser, OperationInfo opInfo)
        {
            UserStatus status = UserStatus.Logoff;
            UserChangeInfo changeInfo = new UserChangeInfo(logoffUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            if (this.Logoffed != null)
            {
                this.Logoffed(logoffUser, opInfo);
            }
        }

        public virtual void Activate(User activateUser, OperationInfo opInfo)
        {
            UserStatus status = UserStatus.Normal;
            UserChangeInfo changeInfo = new UserChangeInfo(activateUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            if (this.Activated != null)
            {
                this.Activated(activateUser, opInfo);
            }
        }

        public void OnCreating(UserCreateInfo createInfo)
        {
            if (this.Creating != null)
            {
                this.Creating(createInfo);
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

        public void OnChanging(UserChangeInfo changeInfo)
        {
            if (this.Changing != null)
            {
                this.Changing(changeInfo.ChangeUser, changeInfo);
            }
        }

        public void OnChanged(UserChangeInfo changeInfo)
        {
            changeInfo.ChangeUser.Change(changeInfo);
            this.CacheContainer.UserCacheManager.Change(changeInfo.ChangeUser, changeInfo.Snapshot);
            if (this.Changed != null)
            {
                this.Changed(changeInfo.ChangeUser, changeInfo);
            }
        }

        public void OnDeleted(User user, OperationInfo opInfo)
        {
            this.CacheContainer.UserCacheManager.Remove(user);
            if(this.Deleted != null)
            {
                this.Deleted(user, opInfo);
            }
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
