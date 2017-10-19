using EIM.Exceptions;
using Newtonsoft.Json;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Api
{
    public abstract class BaseApi
    {
        public BaseApi(JsonServiceClient client, ApiManagerBase apiManager)
        {
            this.Client = client;
            this.ApiManager = apiManager;
        }

        protected JsonServiceClient Client { set; get; }

        protected ApiManagerBase ApiManager { set; get; }

        protected TResponse Get<TResponse>(object requestDto)
        {
            try
            {
                return this._Get<TResponse>(requestDto);
            }
            catch
            {
                //执行出错检查可用服务器重新执行
                CheckServerResult checkServerResult = this.ApiManager.CheckServer();
                if (checkServerResult == CheckServerResult.ChangedServer)
                {
                    return this._Get<TResponse>(requestDto);
                }
                else
                {
                    throw;
                }
            }
        }

        protected virtual TResponse _Get<TResponse>(object requestDto)
        {
            try
            {
                TResponse response = this.Client.Post<TResponse>(requestDto);
                return response;
            }
            catch (Exception ex)
            {
                if (requestDto != null)
                {
                    this.WriteLog(ex, requestDto);
                }
                throw ex;
            }
        }

        protected TResponse Post<TResponse>(object requestDto)
        {
            try
            {
                return this._Post<TResponse>(requestDto);
            }
            catch
            {
                //执行出错检查可用服务器重新执行
                CheckServerResult checkServerResult = this.ApiManager.CheckServer();
                if (checkServerResult == CheckServerResult.ChangedServer)
                {
                    return this._Post<TResponse>(requestDto);
                }
                else
                {
                    throw;
                }
            }
        }

        protected virtual TResponse _Post<TResponse>(object requestDto)
        {
            try
            {
                return this.Client.Post<TResponse>(requestDto);
            }
            catch (Exception ex)
            {
                if (requestDto != null)
                {
                    this.WriteLog(ex, requestDto);
                }
                throw ex;
            }
        }

        protected void StrongerPost(object requestDto)
        {
            try
            {
                this.Post(requestDto);
            }
            catch(Exception ex)
            {
                ServiceLogger.Error(ex);
                this.Post(requestDto);
            }
        }

        protected void Post(object requestDto)
        {
            try
            {
                this._Post(requestDto);
            }
            catch
            {
                //执行出错检查可用服务器重新执行
                CheckServerResult checkServerResult = this.ApiManager.CheckServer();
                if (checkServerResult == CheckServerResult.ChangedServer)
                {
                    this._Post(requestDto);
                }
                else
                {
                    throw;
                }
            }
        }

        protected virtual void _Post(object requestDto)
        {
            try
            {
                this.Client.Post<object>(requestDto);
                EIMLog.Logger.Debug("Api Post执行成功");
            }
            catch (Exception ex)
            {
                if (requestDto != null)
                {
                    this.WriteLog(ex, requestDto);
                }

                throw ex;
            }
        }

        protected virtual void WriteLog(Exception ex, object requestDto)
        {
            if (requestDto != null)
            {
                StackTrace trace = new StackTrace(true);
                string errorLog = string.Format("{0}, requestDtoType: {1}, requestDto: {2}, 堆栈: {3}",
                    ex.Message,
                    requestDto.GetType().Name,
                    JsonConvert.SerializeObject(requestDto),
                    trace.ToString());

                if (ExceptionHelper.IsValidateException(ex))
                {
                    ServiceLogger.Info(ex, requestDto);
                }
                else
                {
                    EIMLog.Logger.Error(errorLog, ex);
                }
            }
        }

        protected virtual void WriteWarnLog(object requestDto, string message)
        {
            if (requestDto != null)
            {
                StackTrace trace = new StackTrace(true);
                string errorLog = string.Format("{0}, requestDtoType: {1}, requestDto: {2}, 堆栈: {3}",
                    message,
                    requestDto.GetType().Name,
                    JsonConvert.SerializeObject(requestDto),
                    trace.ToString());

                EIMLog.Logger.Warn(errorLog);
            }
        }
    }
}
