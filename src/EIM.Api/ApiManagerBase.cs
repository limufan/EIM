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
    public class ApiManagerBase
    {
        public ApiManagerBase(string serviceUri)
        {
            if (string.IsNullOrEmpty(serviceUri))
            {
                throw new ArgumentNullException("serviceUri");
            }

            this._checkServerLock = new object();
            this.ServiceUriList = serviceUri.Split(',').ToList();
            this.ObjectMapper = new ObjectMapper();

            this.CreateApi(this.ServiceUriList.First());

            this._configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.ConfigFileName);
            if (!File.Exists(this._configPath))
            {
                FileStream stream = File.Create(this._configPath);
                stream.Close();
            }
            this.Config();

        }

        string _configPath;
        object _checkServerLock;

        protected virtual string ConfigFileName
        {
            get
            {
                return "EIM_api.config";
            }
        }

        public List<string> ServiceUriList { set; get; }

        public ObjectMapper ObjectMapper { private set; get; }

        public ApiStatus Status 
        { 
            set; 
            get; 
        }

        public string ServiceUri
        {
            set
            {
                this.ServiceClient.SetBaseUri(value);
            }
            get
            {
                return this.ServiceClient.BaseUri;
            }
        }

        public bool CanRad
        {
            get
            {
                if (this.Status == ApiStatus.Enabled)
                {
                    return true;
                }

                return false;
            }
        }

        public bool CanWrite
        {
            get
            {
                if (this.Status != ApiStatus.Disabled)
                {
                    return true;
                }

                return false;
            }
        }

        public JsonServiceClient ServiceClient { private set; get; }

        public ManagerApi ManagerApi { private set; get; }

        protected virtual void CreateApi(string serviceUrl)
        {
            this.ServiceClient = new JsonServiceClient(serviceUrl);
            this.ServiceClient.Timeout = new TimeSpan(0, 0, 6);
#if DEBUG
            this.ServiceClient.Timeout = new TimeSpan(0, 1, 0);
#endif

            this.ManagerApi = new ManagerApi(this.ServiceClient);

        }

        protected T CreateApi<T>()
        {
            T api = (T)Activator.CreateInstance(typeof(T), this.ServiceClient, this);

            return api;
        }

        public CheckServerResult CheckServer()
        {
            lock(this._checkServerLock)
            {
                ServerStatus status = this.ManagerApi.GetServerStatus(this.ServiceUri);
                if (status == ServerStatus.Disable)
                {
                    foreach (string serviceUri in this.ServiceUriList.Where(uri => uri != this.ServiceUri).ToList())
                    {
                        if (this.ChangeServer(serviceUri))
                        {
                            return CheckServerResult.ChangedServer;
                        }
                    }

                    EIMLog.Logger.Warn("CheckServer找不到可用的服务器");
                    this.Disable();
                    return CheckServerResult.Disable;
                }
            }
            return CheckServerResult.Enable;
        }

        public bool ChangeServer(string serviceUri)
        {
            ServerStatus status = this.ManagerApi.GetServerStatus(serviceUri);
            if (status != ServerStatus.Disable)
            {
                this.ServiceUri = serviceUri;
                this.RefreshStatus();
                this.SaveConfig();
                EIMLog.Logger.Info("Change Server 成功, Uri: " + serviceUri);
                return true;
            }

            EIMLog.Logger.Warn("Change Server失败, Uri: " + serviceUri);
            return false;
        }

        public void Enable()
        {
            this.RefreshStatus();
            this.CheckServer();
        }

        public void Disable()
        {
            this.Status = ApiStatus.Disabled;
        }

        public void RefreshStatus()
        {
            ServerStatus serverStatus = this.ManagerApi.GetServerStatus();

            if (serverStatus == ServerStatus.Disable)
            {
                this.Status = ApiStatus.Disabled;
            }
            else if (serverStatus == ServerStatus.Loading)
            {
                this.Status = ApiStatus.Loading;
            }
            else
            {
                this.Status = ApiStatus.Enabled;
            }

            if (this.Status == ApiStatus.Loading)
            {
                this.NewThreadCheckLoaded();
            }
        }

        public void Reload()
        {
            ServerStatus serverStatus = this.ManagerApi.GetServerStatus();
            if (serverStatus == ServerStatus.Enable)
            {
                this.Status = ApiStatus.Loading;
                this.ManagerApi.Reload();

                this.NewThreadCheckLoaded();
            }
        }

        private void NewThreadCheckLoaded()
        {
            Thread thread = new Thread(this.CheckLoaded);
            thread.Start();
        }

        private void CheckLoaded()
        {
            while (true)
            {
#if  DEBUG
                Thread.Sleep(1000);
#else
                Thread.Sleep(1000 * 20);
# endif
                ServerStatus status = this.ManagerApi.GetServerStatus();

                if (status == ServerStatus.Enable)
                {
                    this.Status = ApiStatus.Enabled;
                    break;
                }
            }
        }

        public void SaveConfig()
        {
            ApiConfig config = new ApiConfig();
            config.enabled = this.Status != ApiStatus.Disabled;
            config.serviceUri = this.ServiceUri;

            string configJson = JsonConvert.SerializeObject(config);
            File.WriteAllText(this._configPath, configJson);
        }

        public void ResetConfig()
        {
            File.WriteAllText(this._configPath, "");
            this.Config();
        }

        public void Config()
        {
            try
            {
                string configJson = File.ReadAllText(this._configPath);
                ApiConfig apiConfig;
                if(!JsonConvertHelper.TryConvert<ApiConfig>(configJson, out apiConfig))
                {
                    //默认启用api
                    this.Enable();
                    return;
                }

                if (!string.IsNullOrEmpty(apiConfig.serviceUri))
                {
                    this.ServiceUri = apiConfig.serviceUri;
                }

                if (apiConfig.enabled)
                {
                    this.Enable();
                }
                else
                {
                    this.Disable();
                }
            }
            catch(Exception ex)
            {
                EIMLog.Logger.Error(ex.Message, ex);
            }
        }
    }
}
