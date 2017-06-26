using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Exceptions
{
    [Serializable]
    public class LoadByWhereCountOverflowException : EIMException
    {
        public LoadByWhereCountOverflowException(long count, string where)
        {
            this.EIMExceptionMessage = string.Format("加载数据太多，加载数据：{0}，加载条件:{1}", count, where);
        }


        public LoadByWhereCountOverflowException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
