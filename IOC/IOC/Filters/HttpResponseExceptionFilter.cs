using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using IOC.Exceptions;

namespace IOC.Filters
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is CoreException httpResponseException)
            {
                context.Result = new ObjectResult(httpResponseException.Message)
                {
                    StatusCode = (int)httpResponseException.StatusCode
                };

                context.ExceptionHandled = true;
            }
        }
    }
}
