using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM.Api
{
    [SkipServerStatusValidate]
    public class __ReloadDTO__
    {

    }

    [SkipServerStatusValidate]
    public class __DisableServerDTO__
    {

    }

    [SkipServerStatusValidate]
    public class __EnableServerDTO__
    {

    }

    [SkipServerStatusValidate]
    public class __GetServerStatusDTO__
    {

    }

    public class ManagerApi
    {
        public ManagerApi(JsonServiceClient client)
        {
            this.Client = client;
        }

        protected JsonServiceClient Client { set; get; }

        public void Reload()
        {
            this.Client.Post<object>(new __ReloadDTO__());
        }

        public void DisableServer()
        {
            this.Client.Post<object>(new __DisableServerDTO__());
        }

        public void DisableServer(string serviceUri)
        {
            JsonServiceClient client = new JsonServiceClient(serviceUri);
            client.Timeout = new TimeSpan(0, 0, 2);
            client.Post<object>(new __DisableServerDTO__());
        }

        public void EnableServer()
        {
            this.Client.Post<object>(new __EnableServerDTO__());
        }

        public void EnableServer(string serviceUri)
        {
            JsonServiceClient client = new JsonServiceClient(serviceUri);
            client.Timeout = new TimeSpan(0, 0, 2);
            client.Post<object>(new __EnableServerDTO__());
        }

        public ServerStatus GetServerStatus()
        {
            return this.GetServerStatus(this.Client.BaseUri);
        }

        public ServerStatus GetServerStatus(string serviceUri)
        {
            try
            {
                JsonServiceClient client = new JsonServiceClient(serviceUri);
                client.Timeout = new TimeSpan(0, 0, 2);
                return client.Post<ServerStatus>(new __GetServerStatusDTO__());
            }
            catch
            {
                return ServerStatus.Disable;
            }
        }

    }
}
