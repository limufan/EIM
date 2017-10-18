using EIM.Core;
using EIM.Core.Events;
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
            Instance = new ServiceManager(new BusinessManager());
        }

        public static ServiceManager Instance { set; get; }

        public ServiceManager(BusinessManager businessManager)
        {
            this.BusinessManager = businessManager;
        }


        public ServiceModelMapperFactory ServiceMapperFactory { set; get; }

        public BusinessManager BusinessManager { set; get; }

        public EventManager EventManager { set; get; }
    }
}
