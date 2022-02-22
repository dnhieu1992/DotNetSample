using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ASPNetHandleError.Controllers
{
    //[AllowAnonymous]
    //[ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [Route("/Error")]
        public ApplicationErrorResponse Index()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return new ApplicationErrorResponse()
            {
                Type = exception.GetType().Name,
                StackTrace = exception.Error.StackTrace,
                ErrorMessages = exception.Error.Message
            };
        }
    }
}
