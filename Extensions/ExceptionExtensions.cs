
using Microsoft.AspNetCore.Builder;
using review.Middlewares;

namespace review.Common.Extensions
{
    public static class ExceptionExtensions
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}