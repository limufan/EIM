using EIM.Business;
using EIM.Business.Org;
using EIM.Data;
using EIM.Data.DataModelProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Core.Events
{
    public class UserEvents
    {
        public UserEvents()
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

        public virtual void OnCreating(UserCreateInfo createInfo)
        {
            if (this.Creating != null)
            {
                this.Creating(createInfo);
            }
        }

        public virtual void OnCreated(User user, UserCreateInfo createInfo)
        {
            if (this.Created != null)
            {
                this.Created(user, createInfo);
            }
        }

        public virtual void OnChanging(UserChangeInfo changeInfo)
        {
            if (this.Changing != null)
            {
                this.Changing(changeInfo.ChangeUser, changeInfo);
            }
        }

        public virtual void OnChanged(UserChangeInfo changeInfo)
        {
            if (this.Changed != null)
            {
                this.Changed(changeInfo.ChangeUser, changeInfo);
            }
        }

        public virtual void OnDeleted(User user, OperationInfo opInfo)
        {
            if (this.Deleted != null)
            {
                this.Deleted(user, opInfo);
            }
        }

        public virtual void OnPasswordReseted(User user, OperationInfo opInfo)
        {
            if (this.PasswordReseted != null)
            {
                this.PasswordReseted(user, opInfo);
            }
        }

        public virtual void OnLocked(User user, OperationInfo opInfo)
        {
            if (this.Locked != null)
            {
                this.Locked(user, opInfo);
            }
        }

        public virtual void OnActivated(User user, OperationInfo opInfo)
        {
            if (this.Activated != null)
            {

                this.Activated(user, opInfo);
            }
        }

        public virtual void OnLogoff(User user, OperationInfo opInfo)
        {
            if (this.Logoffed != null)
            {
                this.Logoffed(user, opInfo);
            }
        }
    }
}
