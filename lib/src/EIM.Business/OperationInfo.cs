using EIM.Business.Org;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business
{
    public class OperationInfo
    {
        public OperationInfo()
        {
            this.OperationTime = DateTime.Now;
        }

        public User OperationUser { set; get; }

        public DateTime OperationTime { set; get; }
    }
}
