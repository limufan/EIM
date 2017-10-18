using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM
{
    ///<summary>
    ///数据库表
    ///</summary>
    public class Tables
    {
        /// <summary>
        ///安装服务单
        /// </summary>
        public static string Anzhuangdan = "o_I_Service";

        /// <summary>
        ///维修服务单
        /// </summary>
        public static string Weixiudan = "o_M_Service";

        /// <summary>
        ///安装结算单
        /// </summary>
        public static string AnzhuangJiesuandan = "o_I_Service_Settlement";

        /// <summary>
        ///维修结算单
        /// </summary>
        public static string WeixiuJiesuandan = "o_M_Service_Settlement";

        /// <summary>
        ///安装结算报表
        /// </summary>
        public static string AnzhuangJiesuanBaobiao = "o_I_Service_report";

        /// <summary>
        ///维修结算报表
        /// </summary>
        public static string WeixiuJiesuanBaobiao = "o_M_Service_report";

        /// <summary>
        ///组织挂靠区域
        /// </summary>
        public static string DepartmentArea = "EIM_Organization_Area";

        /// <summary>
        ///组织
        /// </summary>
        public static string Department = "EIM_Organization";

        /// <summary>
        /// 配件
        /// </summary>
        public static string Peijian = "b_Part";

        /// <summary>
        /// 产品
        /// </summary>
        public static string Product = "b_Product";

        /// <summary>
        /// 产品类型
        /// </summary>
        public static string ProductType = "b_ProductType";

        /// <summary>
        /// 产品商品码
        /// </summary>
        public static string ProductCommodityCode = "b_Product_CommodityCode";

        /// <summary>
        /// 用户
        /// </summary>
        public static string User = "EIM_User";

        /// <summary>
        /// 员工
        /// </summary>
        public static string Employee = "EIM_Employee";

    }
}
