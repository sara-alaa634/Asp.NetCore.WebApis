using Ecommerce.ServiceAbstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Presentation.Attributes
{
    public class RadisCacheAttribute : Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute
    {
        private readonly int _durationInMin;

        public RadisCacheAttribute(int DurationInMin = 5)
        {
            _durationInMin = DurationInMin;
        }

        // Async Excuted After / Before Endpoint
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Get Cache Service
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();

            // Check if Cache Data Exsist
            // Create CahceKey

            var cacheKey = CreateCacheKey(context.HttpContext.Request);

            // If Exist => Return Cached Data and Skip Excute Endpoint

            var CacheValue = await cacheService.GetAsync(cacheKey);

            if (CacheValue is not null)
            {
                context.Result = new ContentResult()
                {
                    Content = CacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }


            // iF Not Exsist => Continue to Excute Endpoint
            var executedContext = await next.Invoke();
            if (executedContext.Result is OkObjectResult result)
            {
                // Store Data in Cache
                await cacheService.SetAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(_durationInMin));
            }



        }


        private string CreateCacheKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append($"{request.Path}");  // api/products

            foreach (var item in request.Query.OrderBy(X => X.Key))
            {
                Key.Append($"|{item.Key}-{item.Value}");
            }

            return Key.ToString();
        }
    }
}
