using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM
{
    public class JsonConvertHelper
    {
        public static bool TryConvert<T>(string json, out T t)
        {
            t = default(T);
            try
            {
                t = JsonConvert.DeserializeObject<T>(json);
                if (t == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool TrySerializeObject(object obj, out string json)
        {
            json = "";
            try
            {
                json = JsonConvert.SerializeObject(obj);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static string SerializeObject(object obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch
            {
                return "";
            }
        }
    }
}
