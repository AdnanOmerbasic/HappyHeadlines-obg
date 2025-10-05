using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Enrichers.Span;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Shared
{
    public static class MonitoringService
    {
        public static readonly string ServiceName = Assembly.GetEntryAssembly()!.GetName().Name ?? "Unknown";
        public static ActivitySource ActivitySource = new ActivitySource(ServiceName);
        public static WebApplicationBuilder AddCentralMonitoringService(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenTelemetry()
                .ConfigureResource(res =>
                {
                    res
                     .AddService(serviceName: ServiceName);
                })
                .WithTracing(tracingProvider =>
                {
                    tracingProvider
                     .AddAspNetCoreInstrumentation()
                     .AddOtlpExporter(opts =>
                     {
                         opts.Endpoint = new Uri("http://jaeger:4317");
                     })
                     .AddSource(ActivitySource.Name);
                });

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", Serilog.Events.LogEventLevel.Error)
                .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.Seq("http://seq")
                .Enrich.WithSpan()
                .CreateLogger();

            builder.Host.UseSerilog();

            return builder;
        }
    }
}
