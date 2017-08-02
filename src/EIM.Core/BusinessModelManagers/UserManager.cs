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

        public event TEventHandler<UserCreateInfo> Creating;
        public event TEventHandler<User> Created;
        public virtual event TEventHandler<User, UserChangeInfo> Changing;
        public event TEventHandler<User, UserChangeInfo> Changed;
        public event TEventHandler<User> Deleted;
        public virtual event TEventHandler<User, UserChangeInfo> Logoffed;
        public virtual event TEventHandler<User, UserChangeInfo> Locked;
        public virtual event TEventHandler<User, UserChangeInfo> Activated;
        public virtual event TEventHandler<User, UserChangeInfo> PasswordReseted;

        public User Create(UserCreateInfo createInfo)
        {
            this.OnCreating(createInfo);

            using (BusinessModelProvider<User, UserDataModel> businessModelProvider = 
                this.BusinessModelProviderFactory.CreateProvider<User, UserDataModel>())
            {
                User user = businessModelProvider.Create(createInfo);

                this.OnCreated(user);

                return user;
            }
        }

        public void Change(UserChangeInfo changeInfo)
        {
            this.OnChanging(changeInfo);

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

        public virtual void ResetPassword(User importUser, string newPassword, User opUser)
        {
            string encodedNewPassword = Cryptography.MD5Encode(newPassword);
            this.ImportPassword(importUser, encodedNewPassword, opUser);
        }

        public virtual void ImportPassword(User importUser, string password, User operationUser)
        {
            UserChangeInfo changeInfo = new UserChangeInfo(importUser);
            changeInfo.Password = password;

            this.Change(changeInfo);

            if (this.PasswordReseted != null)
            {
                this.PasswordReseted(importUser, changeInfo);
            }
        }

        public virtual void Lock(User lockUser, User operationUser)
        {
            UserStatus status = UserStatus.Lock;
            UserChangeInfo changeInfo = new UserChangeInfo(lockUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            if (this.Locked != null)
            {
                this.Locked(lockUser, changeInfo);
            }
        }

        public virtual void Unlock(User unlockUser, User operationUser)
        {
            UserStatus status = UserStatus.Normal;
            UserChangeInfo changeInfo = new UserChangeInfo(unlockUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            if (this.Activated != null)
            {

                this.Activated(unlockUser, changeInfo);
            }
        }

        public virtual void Logoff(User logoffUser, User operationUser)
        {
            UserStatus status = UserStatus.Logoff;
            UserChangeInfo changeInfo = new UserChangeInfo(logoffUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            if (this.Logoffed != null)
            {
                this.Logoffed(logoffUser, changeInfo);
            }
        }

        public virtual void Activate(User activateUser, User operationUser)
        {
            UserStatus status = UserStatus.Normal;
            UserChangeInfo changeInfo = new UserChangeInfo(activateUser);
            changeInfo.Status = status;

            this.Change(changeInfo);

            if (this.Activated != null)
            {
                this.Activated(activateUser, changeInfo);
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
            if(this.Changed != null)
            {
                this.Changed(changeInfo.ChangeUser, changeInfo);
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
