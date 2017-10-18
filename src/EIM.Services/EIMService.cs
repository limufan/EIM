using EIM.Core;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Services
{
    public class EIMService: Service
    {
        public EIMService()
        {
            this.MapperFactory = ServiceManager.Instance.ServiceMapperFactory;
            this.BusinessManager = ServiceManager.Instance.BusinessManager;
        }

        public ServiceModelMapperFactory MapperFactory { private set; get; }

        public BusinessManager BusinessManager { private set; get; }
    }
}
