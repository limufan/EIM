using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM
{
    /// <summary>
    /// 配件核销单明细审批状态
    /// </summary>
    public enum PeijianHexiaodanMingxiZhuangtai
    {
        /// <summary>
        /// 未审核
        /// </summary>
        WeiShenhe = 0,

        /// <summary>
        /// 已审核
        /// </summary>
        YiShenhe = 1,

        /// <summary>
        /// 已批准
        /// </summary>
        YiPizhun = 2,
    }

}
