using Microsoft.AspNetCore.Mvc.Filters;
using TaskManagement.API.DataTransfer;

namespace TaskManagement.API.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var error = new ErrorResponse()
            {
                StatusCode = 500,
                Message = context.Exception.Message,
            };
        }
    }
}
