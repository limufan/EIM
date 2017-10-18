using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM.Api
{
    public class ServiceLogger
    {
        public static void Info(Exception ex, object obj)
        {
            if (ex is WebServiceException)
            {
                string message = (ex as WebServiceException).ErrorMessage;
                if(obj != null)
                {
                    string log = string.Format("{0}, requestDtoType: {1}, requestDto: {2}",
                            message, obj.GetType().Name, JsonConvert.SerializeObject(obj));

                    EIMLog.Logger.Info(log);
                }
                else
                {
                    EIMLog.Logger.Info(message, ex);
                }
            }
            else
            {
                EIMLog.Logger.Info(ex.Message, ex);
            }
        }

        public static void Error(Exception ex)
        {
            if (ex is WebServiceException)
            {

                EIMLog.Logger.Error((ex as WebServiceException).ErrorMessage, ex);
            }
            else
            {
                EIMLog.Logger.Error(ex.Message, ex);   
            }
        }

        public static void Warn(Exception ex)
        {
            if (ex is WebServiceException)
            {

                EIMLog.Logger.Warn((ex as WebServiceException).ErrorMessage, ex);
            }
            else
            {
                EIMLog.Logger.Warn(ex.Message, ex);
            }
        }
    }
}
