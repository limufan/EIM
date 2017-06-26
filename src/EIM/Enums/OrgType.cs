using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM
{
    /// <summary>
    /// 组织类型
    /// </summary>
    public enum OrgType
    {
        /// <summary>
        /// 集团 0
        /// </summary>
        Gongsi = 0,
        
        /// <summary>
        /// 服务公司节点 -2
        /// </summary>
        FuwuGongsiNode = -2,

        /// <summary>
        /// 服务中心节点 -3
        /// </summary>
        FuwuZhongxinNode = -3,

        /// <summary>
        /// 总部 1
        /// </summary>
        Zongbu = 1,

        /// <summary>
        /// 服务公司 2
        /// </summary>
        FuwuGongsi = 2,

        /// <summary>
        /// 服务中心 3
        /// </summary>
        FuwuZhongxin = 3,

        /// <summary>
        /// 服务网点 6
        /// </summary>
        FuwuWangdian = 6
    }
}
