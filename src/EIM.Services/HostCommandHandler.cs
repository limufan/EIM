using EIM.Core;
using EIM.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Services
{
    public class HostCommandHandler
    {
        public HostCommandHandler(ServiceHost host)
        {
            this._host = host;
        }

        ServiceHost _host;

        public virtual void Process(string cmd)
        {
            string[] args = cmd.Split(' ');
            if (args[0] == "load")
            {
                this._host.Load();
            }
            else if (args[0] == "GC")
            {
                GC.Collect();
                Console.WriteLine("GC Collected");
            }
            else if (args[0] == "configlog")
            {
                EIMLog.Config();
                Console.WriteLine("配置完成。");
            }
            else if (args[0] == "clear")
            {
                Console.Clear();
                Console.WriteLine("服务已启动。");
            }
            else if (args[0] == "disable")
            {
                this._host.Disable();
                Console.WriteLine("服务Disabled");
            }
            else if (args[0] == "enable")
            {
                this._host.Enable();
                Console.WriteLine("服务Enabled");
            }
            else if (args[0] == "Excute")
            {
                this.Excute(args[1], args[2], args[3]);
                Console.WriteLine("Excuted");
            }
            else
            {
                MethodInfo method = this.GetType().GetMethod(args[0]);
                if (method == null)
                {
                    Console.WriteLine("命令错误");
                }
                else
                {
                    method.Invoke(this, args.Skip(1).ToArray());

                    Console.WriteLine(args[0] + " Excuted");
                }
            }
        }

        
        private void Excute(string assemblyName, string typeName, string methodName)
        {
            Assembly assembly = Assembly.Load(assemblyName);

            Type type = assembly.GetType(typeName);
            MethodInfo method = type.GetMethod(methodName);
            object instance = Activator.CreateInstance(type);

            method.Invoke(instance, new object[] { this._host });
        }
        
    }
}
