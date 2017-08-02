using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Exceptions
{
    [Serializable]
    public class LoadByWhereContainsSqlKeywordflowException : EIMException
    {
        public LoadByWhereContainsSqlKeywordflowException(string where)
        {
            this.ExceptionMessage = string.Format("where 条件中包含了特殊关键字:{0}", where);
        }


        public LoadByWhereContainsSqlKeywordflowException(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }
    }
}
