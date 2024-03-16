using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Api
{
    public static class ExceptionsHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionsHandlerMiddleware>();
            return app;
        }
    }
}
