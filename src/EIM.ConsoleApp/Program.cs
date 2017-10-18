using EIM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EIM.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!SetConsoleCtrlHandler(ConsoleCtrlHandler, true))
            {
                Console.WriteLine("无法注册系统事件!\n");
            }

            ServiceHost host = new ServiceHost(typeof(UserService).Assembly);
            var listeningOn = ConfigurationManagerHelper.GetValue("EIM_Service_Url");
            host.Init().Start(listeningOn);
            host.Load();
            Console.WriteLine("服务已启动。");

            HostCommandHandler commandHandler = new HostCommandHandler(host);

            while (true)
            {
                string cmd = Console.ReadLine();
                if (string.IsNullOrEmpty(cmd))
                {
                    continue;
                }
                if (cmd == "exit")
                {
                    break;
                }

                try
                {
                    commandHandler.Process(cmd);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    EIMLog.Logger.Error(ex.Message, ex);
                }
            }
        }

        const int CTRL_C_EVENT = 0;
        const int CTRL_BREAK_EVENT = 1;
        const int CTRL_CLOSE_EVENT = 2;
        const int CTRL_LOGOFF_EVENT = 5;
        const int CTRL_SHUTDOWN_EVENT = 6;

        public static bool ConsoleCtrlHandler(int ctrlType)
        {
            switch (ctrlType)
            {
                case CTRL_C_EVENT:
                    return true;
            }
            return false;
        }
        public delegate bool ConsoleCtrlHandlerDelegate(int ctrlType);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool SetConsoleCtrlHandler(ConsoleCtrlHandlerDelegate handler, bool add);
    }
}
