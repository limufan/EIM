using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM.Pinyin
{
    public class DimingPinyinProvider : PinyinProvider
    {
        static Dictionary<string, string> _DimingZikuPinyinDic;

        static DimingPinyinProvider()
        {
            _DimingZikuPinyinDic = ZikuLoader.LoadZiku("地名字库");
        }

        public override bool TryGetPinyin(char hanzi, out string pinyin)
        {
            pinyin = string.Empty;

            if (_DimingZikuPinyinDic.ContainsKey(hanzi.ToString()))
            {
                pinyin = _DimingZikuPinyinDic[hanzi.ToString()];

                return true;
            }

            return base.TryGetPinyin(hanzi, out pinyin);
        }
    }
}
