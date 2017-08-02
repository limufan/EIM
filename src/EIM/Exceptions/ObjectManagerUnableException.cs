using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Exceptions
{
    [Serializable]
    public class ObjectManagerUnableException : EIMException
    {
        public ObjectManagerUnableException(string status)
        {
            this.ExceptionMessage = "对象状态异常:" + status.ToString() + ",稍后访问......";
        }


        public ObjectManagerUnableException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
