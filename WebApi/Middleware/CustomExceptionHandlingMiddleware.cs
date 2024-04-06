using System;
using System.Threading.Tasks;
using Business.Validation.Exceptions.BadRequestExceptions;
using Business.Validation.Exceptions.NotFoundExceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace WebApi.Middleware
{
    public class CustomExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            #pragma warning disable CA1031
            catch (Exception ex)
            {
                httpContext.Response.ContentType = "application/json";

                (httpContext.Response.StatusCode, string message) = ex switch
                {
                    NotFoundException => (StatusCodes.Status404NotFound, ex.Message),
                    BadRequestException => (StatusCodes.Status400BadRequest, ex.Message),
                    _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
                };

                var result = JsonConvert.SerializeObject(new 
                {
                    httpContext.Response.StatusCode,
                    message 
                });

                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}