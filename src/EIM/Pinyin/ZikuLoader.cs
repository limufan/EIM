using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM.Pinyin
{
    /// <summary>
    /// 基本字库
    /// </summary>
    public class ZikuLoader
    {
        public static Dictionary<string, string>  LoadZiku(string name)
        {
            Type type = typeof(ZikuLoader);

            Dictionary<string, string> zikuPinyinDic = new Dictionary<string, string>();

            string _namespace = type.Namespace;

            Assembly assembly = type.Assembly;
            string resourceName = string.Format("{0}.{1}.txt", type.Namespace, name);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string zikuSource = sr.ReadToEnd();
                    string[] zikuList = zikuSource.Split(';');
                    foreach (string ziku in zikuList)
                    {
                        if(string.IsNullOrEmpty(ziku))
                        {
                            continue;
                        }
                        string[] zikuPinyin = ziku.Trim().Split('=');
                        zikuPinyinDic.Add(zikuPinyin[0], zikuPinyin[1]);
                    }
                }
            }

            return zikuPinyinDic;
        }
    }
}
