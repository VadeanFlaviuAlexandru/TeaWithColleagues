using System;
using System.Net;

namespace IOC.Exceptions
{
    public class CoreException : Exception
    {
        public CoreException(string message, HttpStatusCode statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public CoreException(HttpStatusCode statusCode) : base("")
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; set; }
    }
}
