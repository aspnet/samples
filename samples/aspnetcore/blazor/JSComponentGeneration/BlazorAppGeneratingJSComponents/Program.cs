using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System;
using JSComponentGeneration.Angular;
using JSComponentGeneration.React;
using BlazorAppGeneratingJSComponents;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.RegisterForAngular<Counter>();
builder.RootComponents.RegisterForReact<Counter>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
