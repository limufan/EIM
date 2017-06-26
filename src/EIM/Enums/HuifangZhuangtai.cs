using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM
{
    /// <summary>
    /// 回访状态
    /// </summary>
    public enum HuifangZhuangtai
    {
        /// <summary>
        /// 未回访 0
        /// </summary>
        Weihuifang = 0,

        /// <summary>
        /// 已回访 1
        /// </summary>
        Yihuifang = 1,

        /// <summary>
        /// 待定 2
        /// </summary>
        Daiding = 2,

        /// <summary>
        /// 待定已回复 4
        /// </summary>
        DaidingYihuifu = 4
    }

    public partial class EnumTextMapper
    {
        public static string GetText(HuifangZhuangtai? zhuangtai)
        {
            switch (zhuangtai)
            {
                case HuifangZhuangtai.Weihuifang:
                    return "未回访";
                case HuifangZhuangtai.Yihuifang:
                    return "已回访";
                case HuifangZhuangtai.Daiding:
                    return "待定";
                case HuifangZhuangtai.DaidingYihuifu:
                    return "待定已回复";
            }
            return "";
        }
    }
}
