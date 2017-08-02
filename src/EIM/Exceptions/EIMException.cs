using System;
using System.Runtime.Serialization;

namespace EIM.Exceptions
{
    [Serializable]
    public class EIMException : ApplicationException
    {
        public EIMException()
        {
            this.ExceptionMessage = "操作异常!";
        }

        public EIMException(string message)
        {
            this.ExceptionMessage = message;
        }

        public EIMException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.ExceptionMessage = info.GetString("EIMExceptionMessage");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EIMExceptionMessage", this.ExceptionMessage);
        }

        protected string ExceptionMessage { set; get; }

        public override string Message
        {
            get
            {
                return this.ExceptionMessage;
            }
        }
    }
}
