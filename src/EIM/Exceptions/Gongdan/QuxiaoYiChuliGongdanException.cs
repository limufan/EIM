using System;
using System.Runtime.Serialization;

namespace EIM.Exceptions
{
    [Serializable]
    public class QuxiaoYiChuliGongdanException : ValidateException
    {
        public QuxiaoYiChuliGongdanException() :
            base("不能取消已处理的服务单")
        {
            
        }


        public QuxiaoYiChuliGongdanException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
