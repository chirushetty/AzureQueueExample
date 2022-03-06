using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AzureQueueExample;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace AzureQueueExample
{
    public class Startup : FunctionsStartup
    {
        public IConfiguration Configuration { get; set; }
        public override void Configure(IFunctionsHostBuilder builder) => ConfigureServices(builder.Services);

        private void ConfigureServices(IServiceCollection services)
        {
            var builder = services.BuildServiceProvider();
            Configuration = builder.GetRequiredService<IConfiguration>();
            var val = Configuration["APPINSIGHT_INSTRUMENTATIONKEY"];
            services.AddLogging();


            services
                .AddMvcCore();

            services.AddInfrastructure();
        }
    }
}
