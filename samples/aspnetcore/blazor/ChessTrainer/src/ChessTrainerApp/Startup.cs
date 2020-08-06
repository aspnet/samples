using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MjrChess.Engine;
using MjrChess.Trainer.Data;
using MjrChess.Trainer.Services;

namespace MjrChess.Trainer
{
    public class Startup
    {
        private const string EnableCompressionKey = "EnableResponseCompression";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Register AAD B2C authentication scheme
            services.AddAuthentication(AzureADB2CDefaults.AuthenticationScheme)
                .AddAzureADB2C(options => Configuration.Bind("AzureAdB2C", options));

            // Add data repository services
            services.AddChessTrainerData(Configuration.GetConnectionString("PuzzleDatabase"));

            // Add services
            services.AddScoped<IPuzzleService, PuzzleService>();
            services.AddScoped<IHistoryService, HistoryService>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            // Add response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            });

            // Health checks (so that a load balancer can easily determine whether the site is up and ready)
            services.AddHealthChecks();

            // Even though this is transient, it's useful to register as a service so that it can be more
            // easily swapped out in the future if the app ever expands to support multiple different engines
            services.AddTransient<ChessEngine>();

            // Add AppInsights services
            services.AddApplicationInsightsTelemetry(Configuration["ApplicationInsights:InstrumentationKey"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, TelemetryConfiguration aiConfig)
        {
            // Issue: https://github.com/dotnet/aspnetcore/issues/18865
            app.UseRewriter(new RewriteOptions().AddRedirect("AzureADB2C/Account/SignedOut", "/"));

            // Apply data migrations, if necessary
            app.ApplicationServices.ApplyDataMigrations();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                aiConfig.DisableTelemetry = true;
            }
            else
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            if (bool.TryParse(Configuration[EnableCompressionKey], out var useResponseCompression) && useResponseCompression)
            {
                app.UseResponseCompression();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapHealthChecks("/hc");
                endpoints.MapControllers(); // For signin/signout endpoints
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
