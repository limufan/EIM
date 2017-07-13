using System;
using System.Runtime.Serialization;

namespace EIM.Exceptions
{
    [Serializable]
    public class EIMException : ApplicationException
    {
        public EIMException()
        {
            this.EIMExceptionMessage = "操作异常!";
        }

        public EIMException(string message)
        {
            this.EIMExceptionMessage = message;
        }

        public EIMException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            this.EIMExceptionMessage = info.GetString("EIMExceptionMessage");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("EIMExceptionMessage", this.EIMExceptionMessage);
        }

        protected string EIMExceptionMessage { set; get; }

        public override string Message
        {
            get
            {
                return this.EIMExceptionMessage;
            }
        }
    }
}
