
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using review.Common.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace review.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            if (ex is NotFoundException)
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            else if (ex is DuplicatedException || ex is BadRequestException)
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            else
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errorResult = new ErrorDetails
            {
                Message = ex.InnerException?.Message ?? ex.Message,
                StatusCode = context.Response.StatusCode
            }.ToString();

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(errorResult);
        }
    }
}