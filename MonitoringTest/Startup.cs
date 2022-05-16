using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;


[assembly: FunctionsStartup(typeof(MonitoringTest.Startup))]

namespace MonitoringTest
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {

            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            builder.Services.AddOpenTelemetryTracing((builder) => builder
                .AddAspNetCoreInstrumentation()
                .AddConsoleExporter()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MonitoringTest"))
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri("http://collector.centralus.azurecontainer.io:4318/v1/traces");
                    options.Protocol = OtlpExportProtocol.HttpProtobuf;
                })
            );
        }
    }
}
