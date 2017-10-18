using EIM.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM.Cache
{
    public abstract class CacheCounter<T>
    {
        public abstract bool Count(T t);

        public abstract string Serialize();
    }
}
