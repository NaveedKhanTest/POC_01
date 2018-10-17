using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace POC.API
{
    public class CommonHttpResponseHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CommonHttpResponseHandlerMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.OnStarting(
                (state) =>
                {
                    string apiKey = context.Request.Headers["api-key"];
                    context.Response.Headers.Add("api-key-back", apiKey);
                    context.Response.Headers.Add("server-responsed-timestamp", DateTime.UtcNow.ToString("s") + "Z");
                    return Task.FromResult(0);
                }, context);

            //context.Response.Headers.Add("response-timestamp", DateTime.UtcNow.ToString("s") + "Z");

            await _next(context);
        }

    }
}
