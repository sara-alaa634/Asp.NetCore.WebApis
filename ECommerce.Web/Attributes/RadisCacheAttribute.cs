using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.Web.Attributes
{
    public class RadisCacheAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
      
        // Async Excuted After / Before Endpoint
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
