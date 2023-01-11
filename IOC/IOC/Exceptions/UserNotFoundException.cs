using System.Net;

namespace IOC.Exceptions
{
    internal class UserNotFoundException : CoreException
    {
        public UserNotFoundException(string message, HttpStatusCode statusCode) : base(message, statusCode)
        {
        }

        public UserNotFoundException() : base("User not found", HttpStatusCode.NotFound)
        {
        }
    }
}
