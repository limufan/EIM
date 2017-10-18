using System;
using System.Runtime.Serialization;

namespace EIM.Exceptions
{
    [Serializable]
    public class ObjectFilterSoftException : ValidateException
    {
        public ObjectFilterSoftException(string message)
            :base(message)
        {
            
        }


        public ObjectFilterSoftException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
