﻿using System;
using System.Collections.Generic;

using System.Text;
using System.Runtime.Serialization;

namespace EIM.Exceptions.Org
{
    [Serializable]
    public class SystemGroupCannotModifyException : OrganizationException
    {
        public SystemGroupCannotModifyException()
        {
            this.ExceptionMessage = "系统用户组无法更改!";
        }

        public SystemGroupCannotModifyException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
