using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM
{
    public enum JiesuandanStatus
    {
        /// <summary>
        /// 1未回访
        /// </summary>
        Weihuifang = 1,

        /// <summary>
        /// 2回访待定
        /// </summary>
        HuifangDaiding = 2,

        /// <summary>
        /// 6待定已回复
        /// </summary>
        DaidingYihuifu = 6,

        /// <summary>
        /// 11已回访待审核
        /// </summary>
        YihuifangDaishenhe = 11,

        /// <summary>
        /// 13已审核
        /// </summary>
        Yishenhe = 13,

        /// <summary>
        /// 14领导审批
        /// </summary>
        LingdaoShenpi = 14,

        /// <summary>
        /// 15审核待定
        /// </summary>
        ShenheDaiding = 15,

        /// <summary>
        /// 16审核待定已说明
        /// </summary>
        ShenheDaidingYishuoming = 16,

        /// <summary>
        /// 21已生成结算报表
        /// </summary>
        YishengchengBaobiao = 21
    }
}
