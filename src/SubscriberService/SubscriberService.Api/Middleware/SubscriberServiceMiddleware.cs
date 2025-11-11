using FeatureHubSDK;

namespace SubscriberService.Api.Middleware
{
    internal sealed class SubscriberServiceMiddleware(RequestDelegate next, IClientContext fhClient)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (!fhClient["SubscriberServiceToggle"].IsEnabled)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Subscriber Service is currently disabled.");
                return;
            }
            await next(context);
        }
    }
}
