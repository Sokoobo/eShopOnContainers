﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pivotal.Extensions.Configuration.ConfigServer;
using Steeltoe.Common.Configuration;
using Steeltoe.Extensions.Logging;
using System.IO;

namespace Microsoft.eShopOnContainers.WebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoggerFactory logFactory = new LoggerFactory();
            logFactory.AddConsole(minLevel: LogLevel.Trace);

            BuildWebHost(args, logFactory).Run();
        }

        public static IWebHost BuildWebHost(string[] args, LoggerFactory logfactory) =>
            WebHost.CreateDefaultBuilder(args)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseHealthChecks("/hc")
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    config.AddConfigServer();
                    config.AddInMemoryCollection(PropertyPlaceholderHelper.GetResolvedConfigurationPlaceholders(config.Build(), logfactory?.CreateLogger("PropertyPlaceholderHelper")));
                })
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddDynamicConsole();
                    builder.AddDebug();
                })
                .UseApplicationInsights()
                .Build();
    }
}
