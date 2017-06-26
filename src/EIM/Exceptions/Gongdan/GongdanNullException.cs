using System;
using System.Runtime.Serialization;

namespace EIM.Exceptions
{
    [Serializable]
    public class GongdanNullException : ValidateException
    {
        public GongdanNullException():
            base("无此条服务单")
        {
            
        }


        public GongdanNullException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
