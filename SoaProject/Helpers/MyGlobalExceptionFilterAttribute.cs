
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

//BadRequest exception thrown for any error

namespace SoaProject.Helpers
{
    public class MyGlobalExceptionFilterAttribute  : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {  
            context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            context.Response.ReasonPhrase = "Kuch to gadbad hai ! Daya , pata lagao !";
        }
    }
}