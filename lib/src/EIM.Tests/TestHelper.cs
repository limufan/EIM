using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Tests
{
    public class TestHelper
    {
        public static Result Invoke<Result>(Func<Result> func)
        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(2000);
                try
                {
                    return func();
                }
                catch (Exception ex)
                {
                    //一分钟后还获取不到测试失败
                    if (i > 40)
                    {
                        throw ex;
                    }
                }
            }

            return func();
        }
    }
}
