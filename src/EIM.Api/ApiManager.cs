using EIM.Api.Org;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EIM.Api
{
    public class ApiManager:  ApiManagerBase
    {
        public static string ServiceUrlConfig
        {
            get
            {
                return ConfigurationManagerHelper.GetValue("EIM_Service_Url", "http://localhost:6261");
            }
        }

        static ApiManager()
        {
            Instance = new ApiManager(ServiceUrlConfig);

        }

        public static ApiManager Instance { set; get; }

        public ApiManager(string serviceUri)
            : base(serviceUri)
        {
            

        }

        public ApiManager()
            : base(ServiceUrlConfig)
        {


        }

        public UserApi UserApi { private set; get; }

        protected override void CreateApi(string serviceUrl)
        {
            base.CreateApi(serviceUrl);

            this.UserApi = this.CreateApi<UserApi>();
        } 
    }
}
