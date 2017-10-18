using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM
{
    public class EIMLog
    {
        static EIMLog()
        {
            _Config();
#if DEBUG
            Logger.Info("---------------DEBUG BUILD---------------------");
#else
            Logger.Info("---------------RELEASE BUILD---------------------");
#endif
        }

        public static ILog Logger { private set; get; }

        private static void _Config()
        {
            string log4netConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "eimLog4net.config");
            FileInfo configFileInfo = new FileInfo(log4netConfigFile);
            log4net.Config.XmlConfigurator.Configure(configFileInfo);
            Logger = log4net.LogManager.GetLogger("logger");
        }

        public static void Config()
        {
            _Config();

            EIMLog.Logger.Error("Error");
            EIMLog.Logger.Debug("Debug");
            EIMLog.Logger.Fatal("Fatal");
            EIMLog.Logger.Info("Info");
            EIMLog.Logger.Warn("Warn");
        }
    }
}
