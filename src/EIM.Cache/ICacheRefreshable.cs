using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Cache
{
    public interface ICacheRefreshable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheInfo">新的缓存信息</param>
        /// <returns>刷新之前的快照</returns>
        T Refresh(T cacheInfo);
    }
}
