using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace EIM
{
    public class ConfigurationManagerHelper
    {
        public static int GetIntValue(string name)
        {
            int value = 0;
            if(ConfigurationManager.AppSettings[name] != null)
            {
                int.TryParse(ConfigurationManager.AppSettings[name], out value);
            }
            return value;
        }

        public static bool GetBoolValue(string name)
        {
            bool value = false;
            if(ConfigurationManager.AppSettings[name] != null)
            {
                bool.TryParse(ConfigurationManager.AppSettings[name], out value);
            }
            return value;
        }

        public static string GetValue(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }

        public static string GetValue(string name, string defaultValue)
        {
            if(ConfigurationManager.AppSettings[name] != null)
            {
                return ConfigurationManager.AppSettings[name];
            }
            return defaultValue;
        }

    }
}
