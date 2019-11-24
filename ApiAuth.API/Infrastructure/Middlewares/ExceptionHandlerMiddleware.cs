using ApiAuth.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace ApiAuth.API.Infrastructure.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private const string JsonContentType = "application/json";
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var httpStatusCode = ConfigureExceptionTypes(exception);

                context.Response.StatusCode = httpStatusCode;
                context.Response.ContentType = JsonContentType;

                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new ErrorViewModel
                    {
                        Message = exception.Message ?? "Something wrong happened, please try later."
                    }));
                context.Response.Headers.Clear();
            }
        }

        private static int ConfigureExceptionTypes(Exception exception)
        {
            int httpStatusCode;
            if (exception is ValidationException ||
                exception is ArgumentException ||
                exception is InvalidOperationException)
            {
                httpStatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exception is NullReferenceException)
            {

                httpStatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (exception is UnauthorizedAccessException)
            {
                httpStatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                httpStatusCode = (int)HttpStatusCode.InternalServerError;

            }

            return httpStatusCode;
        }
    }
}
