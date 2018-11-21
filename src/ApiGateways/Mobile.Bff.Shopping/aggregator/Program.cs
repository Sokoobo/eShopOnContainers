﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Steeltoe.Extensions.Logging;

namespace Microsoft.eShopOnContainers.Mobile.Shopping.HttpAggregator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(cb =>
                {
                    var sources = cb.Sources;
                    sources.Insert(3, new Microsoft.Extensions.Configuration.Json.JsonConfigurationSource()
                    {
                        Optional = true,
                        Path = "appsettings.localhost.json",
                        ReloadOnChange = false
                    });
                })
                .ResolveConfigurationPlaceholders()
                .ConfigureLogging((ctx, builder) =>
                {
                    builder.ClearProviders();
                    builder.AddConfiguration(ctx.Configuration);
                    builder.AddDynamicConsole();
                    builder.AddDebug();
                })
                .UseStartup<Startup>()
                .Build();
    }
}
