using Elmah;
using System.Net.Http;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace ToDoApp
{
    public class ElmahExceptionLogger : ExceptionLogger
    {
        private const string HttpContextBaseKey = "MS_HttpContext";

        public override void Log(ExceptionLoggerContext context)
        {
            // Retrieve the current HttpContext instance for this request.
            var httpContext = GetHttpContext(context.Request);

            // Wrap the exception in an HttpUnhandledException so that ELMAH can capture the original error page.
            var exceptionToRaise = new HttpUnhandledException(message: null, innerException: context.Exception);

            var signal = httpContext == null ? ErrorSignal.FromCurrentContext() : ErrorSignal.FromContext(httpContext);

            signal.Raise(exceptionToRaise);
        }

        private static HttpContext GetHttpContext(HttpRequestMessage request)
        {
            var contextBase = GetHttpContextBase(request);

            if (contextBase == null)
            {
                return null;
            }

            return ToHttpContext(contextBase);
        }

        private static HttpContextBase GetHttpContextBase(HttpRequestMessage request)
        {
            object value=null;

            if (request?.Properties?.TryGetValue(HttpContextBaseKey, out value) == false)
            {
                return null;
            }

            return value as HttpContextBase;
        }

        private static HttpContext ToHttpContext(HttpContextBase contextBase)
        {
            return contextBase.ApplicationInstance.Context;
        }
    }

}