using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM
{
    /// <summary>
    /// 结算类型
    /// </summary>
    public enum JiesuanType
    {
        /// <summary>
        /// -1 不结算
        /// </summary>
        Bujiesuan = -1,

        /// <summary>
        /// 10 送修
        /// </summary>
        Songxiu = 10,

        /// <summary>
        /// 20 小修
        /// </summary>
        Xiaoxiu = 20,

        /// <summary>
        /// 30 中修
        /// </summary>
        Zhongxiu = 30,

        /// <summary>
        /// 40 大修
        /// </summary>
        Daxiu = 40
    }
}
