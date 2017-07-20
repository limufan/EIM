using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.Org
{
    public class User : IIdCodeProvider
    {
        public int Id { set; get; }

        public string Code { set; get; }

        public string Guid { set; get; }


    }
}
