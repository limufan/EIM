﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Cache
{
    public interface ICache<T>
    {
        void Refresh(T cacheInfo);

        T Clone();
    }
}
