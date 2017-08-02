using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Exceptions
{
    [Serializable]
    public class DataProviderLoadedCountNotEqualException : EIMException
    {
        public DataProviderLoadedCountNotEqualException(int databaseCount, int memoryCount)
        {
            this.ExceptionMessage = string.Format("内存和数据库数目不相等, 数据库：{0}, 内存: {1}", databaseCount, memoryCount);
        }


        public DataProviderLoadedCountNotEqualException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
