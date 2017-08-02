using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace EIM.Exceptions.Org
{
    [Serializable]
    public class PositionChildHasUserDeleteException : OrganizationException
    {
        public PositionChildHasUserDeleteException()
        {
            this.ExceptionMessage = "无法删除职位，子职位中包含有用户!";
        }

        public PositionChildHasUserDeleteException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }

        public override string Message
        {
            get
            {
                if (this.StringResouceProvider != null)
                {
                    return this.StringResouceProvider.GetString("ex_PositionChildHasUserDelete");
                }
                return base.Message;
            }
        }
    }
}
