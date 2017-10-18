using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace EIM
{

    public class JsonWebClient : WebClient
    {
        public JsonWebClient()
        {
            this._cookie = new CookieContainer();
            this.Encoding = UTF8Encoding.UTF8;
            
        }

        CookieContainer _cookie;

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                HttpWebRequest httpRequest = request as HttpWebRequest;
                httpRequest.CookieContainer = this._cookie;
            }
            return request;
        }



        public T Get<T>(string url)
        {
            Stream stream = this.OpenRead(url);
            StreamReader sr = new StreamReader(stream, UTF8Encoding.UTF8);

            string responseText = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(responseText);
        }


        public string Get(string url)
        {
            Stream stream = this.OpenRead(url);
            StreamReader sr = new StreamReader(stream, UTF8Encoding.UTF8);

            return sr.ReadToEnd();
        }


        public T Post<T>(string url, object data)
        {
            Stream stream = this.OpenRead(url);
            StreamReader sr = new StreamReader(stream, UTF8Encoding.UTF8);

            string responseText = sr.ReadToEnd();

            return JsonConvert.DeserializeObject<T>(responseText);
        }
    }
}
