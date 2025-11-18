using Ecommerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace ECommerce.Web.CustomMiddlewares
{
    public class ExceptionHandlerMiddleware
    {
       

        // 1. You Must Inject RequestDelegate as Next Middleware to ur CTOR
        // 2. Must Have InvokeAsync Method with HttpContext Parameter
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate Next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = Next;
            _logger = logger;
        }

        public async Task InvoceAsync(HttpContext context)
        {
			try
            {
                await _next.Invoke(context);
                await HandleNotFoundEndpointAsync(context);
            }
            catch (Exception ex)
			{

               //Logger
               //Logger Message to COnsole
               _logger.LogError(ex, $"Something went wrong");
                var Problem = new ProblemDetails()
                {
                    Title = "An Unexpected Error Occured",
                    Detail = ex.Message,
                    Instance = context.Request.Path ,// URL

                    Status = ex switch
                    {
                        NotFoundException=> StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    }
                };

                context.Response.StatusCode = Problem.Status.Value;

                await context.Response.WriteAsJsonAsync(Problem);


            }
        }

        private static async Task HandleNotFoundEndpointAsync(HttpContext context)
        {
            // 4040 NotFound
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "Resource Not Found",
                    Status = StatusCodes.Status404NotFound,
                    Detail = "The Resourse You Are Looking For Is Not Found",
                    Instance = context.Request.Path // URL
                };
                await context.Response.WriteAsJsonAsync(Problem);
            }
        }
    }
}
