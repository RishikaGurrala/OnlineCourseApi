
using System.Text;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using Microsoft.ApplicationInsights.DataContracts;

namespace OnlineCourseApi.Middlewares
{
    public class RequestBodyLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var method = context.Request.Method;
            context.Request.EnableBuffering();
            if(context.Request.Body.CanRead && (method == HttpMethods.Post || method == HttpMethods.Put))
            {
                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 512, leaveOpen: true
                    );
                var requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
                var requestTelemetry = context.Features.Get<RequestTelemetry>();
                    requestTelemetry?.Properties.Add("RequestBody", requestBody);
                Log.Information("Request:" + requestBody);
            }
            await next(context);
        }
    }
}
