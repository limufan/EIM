using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM.Pinyin
{
    public class PinyinProvider
    {
        static Dictionary<string, string> ZikuPinyinDic;

        static PinyinProvider()
        {
            ZikuPinyinDic = ZikuLoader.LoadZiku("字库");
        }

        public virtual bool TryGetPinyin(char hanzi, out string pinyin)
        {
            pinyin = string.Empty;

            if (ZikuPinyinDic.ContainsKey(hanzi.ToString()))
            {
                pinyin = ZikuPinyinDic[hanzi.ToString()];

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取拼音简码
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public string GetJianma(string str)
        {
            string pinyin;
            string jianma = "";
            foreach (char hanzi in str)
            {
                if(this.TryGetPinyin(hanzi, out pinyin))
                {
                    jianma += pinyin[0];
                }
                else
                {
                    jianma += hanzi.ToString();
                }
            }
            return jianma;
        }

        /// <summary>
        /// 获取全拼
        /// </summary>
        /// <param name="unicodeString"></param>
        /// <returns></returns>
        public string GetQuanpin(string str)
        {
            string pinyin;
            string jianma = "";
            foreach (char hanzi in str)
            {
                if (hanzi == ' ')
                {
                    continue;
                }

                if (this.TryGetPinyin(hanzi, out pinyin))
                {
                    jianma += pinyin;
                }
                else
                {
                    jianma += hanzi.ToString();
                }
            }
            return jianma;
        }

    }
}
