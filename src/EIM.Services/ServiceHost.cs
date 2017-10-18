using Funq;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using ServiceStack.Web;
using Newtonsoft.Json;
using System.Reflection;
using EIM.Exceptions;
using EIM.Api;

namespace EIM.Services
{
    public class ServiceHost : AppSelfHostBase
    {
        public static ServiceHost ServiceHostInstance { private set; get; }

        public ServiceHost(params Assembly[] assembliesWithServices)
            : base("HttpListener Self-Host", assembliesWithServices) 
        {
            ServiceHostInstance = this;

            this.ServiceManager = ServiceManager.Instance;

            this._requestCounter = new Dictionary<Type, long>();
        }

        public ServiceManager ServiceManager { private set; get; }

        Dictionary<Type, long> _requestCounter;

        public override void Configure(Container container) 
        {
#if DEBUG
            this.SetConfig(new HostConfig { DebugMode = true });
            this.Plugins.Add(new RequestLogsFeature());
#endif

            this.GlobalRequestFilters.Add(this.ValidationFilter);
        }

        private void ValidationFilter(IRequest httpReq, IResponse httpRes, object request)
        {
            if (request != null)
            {
                string rawUrl = "";
                if (httpReq != null)
                {
                    rawUrl = httpReq.RawUrl;
                }

                EIMLog.Logger.Debug(string.Format("ValidationFilter {0}: {1}, rawUrl: {2} ",
                    request.GetType().Name, JsonConvert.SerializeObject(request), rawUrl));

                this.ValidateServerStatus(request);

                this.OnRequest(request);

                this.Statistics(request);
            }
        }

        protected virtual void OnRequest(object request)
        {

        }

        private void Statistics(object request)
        {
            Type dtoType = request.GetType();
            if (this._requestCounter.ContainsKey(dtoType))
            {
                this._requestCounter[dtoType]++;
            }
            else
            {
                this._requestCounter.Add(dtoType, 1);
            }

            long count = this._requestCounter[dtoType];
            if (count % 1000 == 0)
            {
                EIMLog.Logger.Info(dtoType + "请求次数：" + count);
            }

            if (count >= 200000)
            {
                EIMLog.Logger.Warn(dtoType + "请求次数：" + count);
                this._requestCounter[dtoType] = 0;
            }
        }

        private void ValidateServerStatus(object request)
        {
            if(ReflectionHelper.HasAttribute<SkipServerStatusValidateAttribute>(request.GetType()))
            {
                return;
            }

            //if (this.Status == ServerStatus.Disable)
            //{
            //    throw new EIMException("服务已禁用!");
            //}
        }

        public override object OnServiceException(ServiceStack.Web.IRequest httpReq, object request, Exception ex)
        {
            if (ex != null)
            {
                string errorMessage = ex.Message;
                string rawUrl = "";
                if (httpReq != null)
                {
                    rawUrl = httpReq.RawUrl;
                }

                if (ex is WebServiceException)
                {
                    WebServiceException webServiceException = ex as WebServiceException;
                    errorMessage = webServiceException.ErrorMessage;
                }
                if (request != null)
                {
                    errorMessage = string.Format("Post 出错{0}, requestType: {1}, request: {2}, rawUrl: {3}",
                             errorMessage,
                             request.GetType().Name,
                             JsonConvert.SerializeObject(request),
                             rawUrl);
                }

                if (ex is ValidateException)
                {
                    EIMLog.Logger.Info(errorMessage, ex);
                }
                else
                {
                    EIMLog.Logger.Error(errorMessage, ex);
                }
            }
            else
            {
                EIMLog.Logger.Error(request);
            }

            return base.OnServiceException(httpReq, request, ex);
        }

        public ServerStatus Status { set; get; }

        public void Disable()
        {
            this.Status = ServerStatus.Disable;
        }

        public void Enable()
        {
            this.Status = ServerStatus.Enable;
        }

        public virtual void Load()
        {
            this.ServiceManager.BusinessManager.CacheProviderManager.Load();

        }
        
    }
}
