using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM
{
    /// <summary>
    /// 配件核销单核销状态
    /// </summary>
    public enum PeijianHexiaodanHexiaoZhuangtai
    {
        /// <summary>
        /// 未核销 0
        /// </summary>
        WeiHexiao = 0,

        /// <summary>
        /// 已核销 1
        /// </summary>
        YiHexiao = 1,

        /// <summary>
        /// 取消核销 2
        /// </summary>
        QuxiaoHexiao = 2,

        /// <summary>
        /// 部分核销 3
        /// </summary>
        BufenHexiao = 3
    }

    /// <summary>
    /// 配件核销单审批状态
    /// </summary>
    public enum PeijianHexiaodanShenpiZhuangtai
    {
        /// <summary>
        /// 未提交 0
        /// </summary>
        WeiTijiao = 0,

        /// <summary>
        /// 待审核 1
        /// </summary>
        DaiShenhe = 1,

        /// <summary>
        /// 待批准 2
        /// </summary>
        DaiPizhun = 2,

        /// <summary>
        /// 已批准 3
        /// </summary>
        YiPizhun = 3,

        /// <summary>
        /// 不批准 4
        /// </summary>
        BuPizhun = 4,

        /// <summary>
        /// 待签收 5
        /// </summary>
        DaiQianshou = 5,

        /// <summary>
        /// 审核不通过 6
        /// </summary>
        ShenheBuTongguo = 6,

        /// <summary>
        /// 已签收 8
        /// </summary>
        YiQianshou = 8,

        /// <summary>
        /// 部分签收 9
        /// </summary>
        BufenQianshou = 9,

        /// <summary>
        /// 已完成 10
        /// </summary>
        YiWancheng = 10
    }
    
    
    /// <summary>
    /// 配件核销单流程状态
    /// </summary>
    public enum PeijianHexiaodanLiuchengZhuangtai
    {
        /// <summary>
        /// 未提交 1
        /// </summary>
        WeiTijiao = 1,

        /// <summary>
        /// 待签收 2
        /// </summary>
        DaiQianshou = 2,

        /// <summary>
        /// 待审核 4
        /// </summary>
        DaiShenhe = 4,

        /// <summary>
        /// 待批准 8
        /// </summary>
        DaiPizhun = 8,

        /// <summary>
        /// 部分核销 16
        /// </summary>
        BufenHexiao = 16,

        /// <summary>
        /// 核销结束 32
        /// </summary>
        HexiaoJieshu = 32
    }

    /// <summary>
    /// 配件核销单状态(搜索时使用的)
    /// </summary>
    public enum PeijianHexiaodanZhuangtai
    {
        /// <summary>
        /// 已签收 -1
        /// </summary>
        YiQianshou = -1,

        /// <summary>
        /// 未核销 0
        /// </summary>
        WeiHexiao = 0,

        /// <summary>
        /// 核销通过 1
        /// </summary>
        HexiaoTongguo = 1,

        /// <summary>
        /// 待批准 2
        /// </summary>
        DaiPizhun = 2,

        /// <summary>
        /// 部分核销 3
        /// </summary>
        BufenHexiao = 4,

        /// <summary>
        /// 核销结束
        /// </summary>
        HexiaoJieshu = 10
    }
}
