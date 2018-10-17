namespace POC.API
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;



    //Ref: https://techblog.dorogin.com/handling-errors-in-aspnet-core-middleware-e39872496d51 
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    //var statusCodeFeature = context.Features.Get<IStatusCodePagesFeature>();
                    //if (statusCodeFeature == null || !statusCodeFeature.Enabled)
                    //{
                    //    //// there's no StatusCodePagesMiddleware in app
                    //    //if (!context.Response.HasStarted)
                    //    //{
                    //    //    var view = new ErrorPage(new ErrorPageModel());
                    //    //    await view.ExecuteAsync(context);
                    //    //}
                    //}
                }
            }
            catch (Exception ex)
            {
                // TODO: stay tuned
                throw;
            }
        }
    }


    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        //Note:  UseExceptionHandler middleware is a built-in middleware that we can use to handle exceptions

        //Global Error Handl
        public CustomExceptionHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleAsyncException(httpContext, ex);
            }
        }

        private static Task HandleAsyncException(HttpContext httpContext, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            string result = string.Empty;
            switch (exception)
            {
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;

                case ValidationException _:
                    code = HttpStatusCode.BadRequest;
                    var valExc = (ValidationException)exception;
                    result = JsonConvert.SerializeObject(valExc);
                    //result = JsonConvert.SerializeObject(new SomeCommonModel() { messages = valExc });
                    break;

                default:
                    code = HttpStatusCode.InternalServerError;
                    var messageList = new List<object>
                    {
                        new  {  Description = exception.ToString() }
                    };
                    result = JsonConvert.SerializeObject(new  { messages = messageList, otherFields = "some thing else" });
                    break;
            }

            httpContext.Response.StatusCode = (int)code;
            //httpContext.Response.ContentType = "application/json";
            return httpContext.Response.WriteAsync(result);
        }
    }



    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }


        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}