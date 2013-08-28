using System.Web.Http.Filters;

namespace Croom.Backend.Infrastructure.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {

        }
    }
}