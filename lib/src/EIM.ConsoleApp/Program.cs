using EIM.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RPCServer rpcServer = new RPCServer();

            Console.WriteLine("server started");
            Console.ReadLine();
        }
    }
}
