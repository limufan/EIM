using EIM.Business;
using EIM.Business.Org;
using EIM.Data;
using EIM.Data.DataModelProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.EventManagers
{
    public class UserEventManager
    {
        public UserEventManager()
        {
            
        }

        public event TEventHandler<UserCreateInfo> Creating;
        public event TEventHandler<User, UserCreateInfo> Created;
        public event TEventHandler<User, UserChangeInfo> Changing;
        public event TEventHandler<User, UserChangeInfo> Changed;
        public event TEventHandler<User, OperationInfo> Deleted;
        public event TEventHandler<User, OperationInfo> Logoffed;
        public event TEventHandler<User, OperationInfo> Locked;
        public event TEventHandler<User, OperationInfo> Activated;
        public event TEventHandler<User, OperationInfo> PasswordReseted;

        public void OnCreating(UserCreateInfo createInfo)
        {
            if (this.Creating != null)
            {
                this.Creating(createInfo);
            }
        }

        public void OnCreated(User user, UserCreateInfo createInfo)
        {
            if (this.Created != null)
            {
                this.Created(user, createInfo);
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
            if (this.Changed != null)
            {
                this.Changed(changeInfo.ChangeUser, changeInfo);
            }
        }

        public void OnDeleted(User user, OperationInfo opInfo)
        {
            if (this.Deleted != null)
            {
                this.Deleted(user, opInfo);
            }
        }

        public void OnPasswordReseted(User user, OperationInfo opInfo)
        {
            if (this.PasswordReseted != null)
            {
                this.PasswordReseted(user, opInfo);
            }
        }

        public void OnLocked(User user, OperationInfo opInfo)
        {
            if (this.Locked != null)
            {
                this.Locked(user, opInfo);
            }
        }

        public void OnActivated(User user, OperationInfo opInfo)
        {
            if (this.Activated != null)
            {

                this.Activated(user, opInfo);
            }
        }

        public void OnLogoff(User user, OperationInfo opInfo)
        {
            if (this.Logoffed != null)
            {
                this.Logoffed(user, opInfo);
            }
        }
    }
}
