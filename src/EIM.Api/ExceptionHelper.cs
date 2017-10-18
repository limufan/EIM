using EIM.Exceptions;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM.Api
{
    public class ExceptionHelper
    {
        public static bool IsValidateException(Exception ex)
        {
            if (ex is WebServiceException)
            {
                WebServiceException webServiceException = ex as WebServiceException;
                if (webServiceException != null && webServiceException.ErrorCode == typeof(ValidateException).Name)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
