using EIM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Services
{
    public class EIMService
    {
        public EIMService()
        {
            this.MapperFactory = ServiceManager.Instance.ServiceMapperFactory;
            this.BusinessManager = ServiceManager.Instance.BusinessManager;
        }

        public ServiceModelMapperFactory MapperFactory { set; get; }

        public BusinessManager BusinessManager { set; get; }
    }
}
