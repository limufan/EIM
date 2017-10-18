using EIM.Pinyin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM
{
    public class HanziPinyinHelper
    {
        static PinyinProvider _PinyinProvider;
        static DimingPinyinProvider _DimingPinyinProvider;

        static HanziPinyinHelper()
        {
            _PinyinProvider = new PinyinProvider();
            _DimingPinyinProvider = new DimingPinyinProvider();
        }

        /// <summary>
        /// 获取拼音简码
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string GetJianma(string str)
        {
            return _PinyinProvider.GetJianma(str);
        }

        /// <summary>
        /// 获取全拼
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string GetQuanpin(string str)
        {
            return _PinyinProvider.GetQuanpin(str);
        }

        /// <summary>
        /// 获取地名拼音简码
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string GetDimingJianma(string str)
        {
            return _DimingPinyinProvider.GetJianma(str);
        }

        /// <summary>
        /// 获取地名全拼
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public static string GetDimingQuanpin(string str)
        {
            return _DimingPinyinProvider.GetQuanpin(str);
        }

    }
}
