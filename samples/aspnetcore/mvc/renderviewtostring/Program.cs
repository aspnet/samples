// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.ObjectPool;

namespace Mvc.RenderViewToString
{
    public class Program
    {
        public static void Main()
        {
            var builder = new HostBuilder()
                .ConfigureWebHost(h => h.UseSetting(WebHostDefaults.ApplicationKey, typeof(Program).Assembly.GetName().Name))
                .ConfigureServices(services =>
                {
                    var diagnosticSource = new DiagnosticListener("Microsoft.AspNetCore");
                    services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
                    services.AddSingleton<DiagnosticListener>(diagnosticSource);
                    services.AddSingleton<DiagnosticSource>(diagnosticSource);
                    services.AddLogging();
                    services.AddMvc();
                    services.AddTransient<RazorViewToStringRenderer>();

                    services.AddSingleton<EmailReportGenerator>();
                });

            var host = builder.Build();

            var emailContent = RenderViewAsync(host.Services).Result;

            Console.WriteLine(emailContent);
        }

        public static Task<string> RenderViewAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.CreateScope())
            {
                var helper = serviceScope.ServiceProvider.GetRequiredService<RazorViewToStringRenderer>();

                var model = new EmailViewModel
                {
                    UserName = "Test User",
                    SenderName = "Contoso",
                    UserData1 = 1,
                    UserData2 = 2
                };

                return helper.RenderViewToStringAsync("/Views/EmailTemplate.cshtml", model);
            }
        }
    }
}
