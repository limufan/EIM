using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EIM.Exceptions.Org
{
    [Serializable]
    public class LicenseException : OrganizationException
    {
        public LicenseException()
        {
            this.ExceptionMessage = "License异常!";
        }

        public LicenseException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            
        }
    }
}
