using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CryptoPrice.CryptoProviders;
using CryptoPrice.Hubs;
using CryptoPrice.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CryptoPrice
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string apiKey = _configuration.GetValue<string>("ApiKey");
            var internalConnectionStorage = new Dictionary<string, List<string>>();
            var internalBackgroundTasks = new Dictionary<string, Task>();
            var internalCancellationTaskSources = new Dictionary<string, CancellationTokenSource>();

            services.AddTransient<ICryptoProvider, CryptoCompareProvider>();
            services.AddTransient<ICryptoProvider>(provider => new CryptoCompareProvider(apiKey));
            services.AddSingleton<IPriceTaskPool, PriceTaskPool>();
            services.AddSingleton<IConnectionGroupStorage>(provider => new ConnectionGroupStorage(internalConnectionStorage));
            services.AddSingleton<IPriceTaskStorage>(provider => new PriceTaskStorage(internalBackgroundTasks, internalCancellationTaskSources));
            services.AddSingleton<IPriceTaskFactory, PriceTaskFactory>();
            services.AddSingleton<IPriceTask, PriceTask>();

            services.AddMvc();
            services.AddCors();
            services.AddSignalR();
            services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }

            app.UseCors(builder =>
                builder
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            );

            app.UseSignalR(routes =>
            {
                routes.MapHub<PriceHub>("/price-hub");
            });

            app.UseMvc();
        }
    }
}
