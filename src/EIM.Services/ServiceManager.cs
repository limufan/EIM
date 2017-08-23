using EIM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Services
{
    public class ServiceManager
    {

        static ServiceManager()
        {
            Instance = new ServiceManager(BusinessManager.Instance);
        }

        public static ServiceManager Instance { set; get; }

        public ServiceManager(BusinessManager businessManager)
        {
            this.BusinessManager = businessManager;
        }


        public ServiceModelMapperFactory ServiceMapperFactory { set; get; }

        public BusinessManager BusinessManager { set; get; }
    }
}
