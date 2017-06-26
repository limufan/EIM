using System;
using System.Runtime.Serialization;

namespace EIM.Exceptions
{
    [Serializable]
    public class QuxiaoDuogeGongdanException : ValidateException
    {
        public QuxiaoDuogeGongdanException():
            base("不能取消多个服务单")
        {
            
        }


        public QuxiaoDuogeGongdanException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
