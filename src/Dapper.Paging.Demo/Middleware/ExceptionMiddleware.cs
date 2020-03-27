using Dapper.Razor.Demo.Errors;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Dapper.Paging.Demo.Middleware
{
    /// <summary>
    /// Handling Errors Globally with the Custom Middleware
    /// </summary>
    /// inspired by: https://code-maze.com/global-error-handling-aspnetcore/
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var errorMsg = $"path: {httpContext.Request.Path}, {ex.Message}";
                Log.Error(errorMsg);
                await HandleExceptionAsync(httpContext, ex).ConfigureAwait(false);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var errorMsg = ex.Message;
            if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
            {
                errorMsg = ex.InnerException.Message;
            }
            return httpContext.Response.WriteAsync(new ApiError(httpContext.Response.StatusCode, errorMsg).ToString());
        }
    }
}
