using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Modules;

namespace ThisNetWorks.OrchardCore.Azure.Queues
{
    public class Startup : StartupBase
    {
        private readonly IShellConfiguration _shellConfiguration;
        
        public Startup(IShellConfiguration shellConfiguration)
        {
            _shellConfiguration = shellConfiguration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions<AzureQueueOptions>();
            var connectionString = _shellConfiguration.GetValue("ThisNetWorks_OrchardCore_Azure_Queues:ConnectionString", String.Empty);
            var createQueues = _shellConfiguration.GetValue("ThisNetWorks_OrchardCore_Azure_Queues:CreateQueues", false);
            services.Configure<AzureQueueOptions>(x => {
                x.ConnectionString = connectionString;
                x.CreateQueues = createQueues;
            });

            services.AddSingleton<IAzureQueueService, AzureQueueService>();
            services.AddSingleton<IAzureQueueResolver, AzureQueueResolver>();
            services.AddScoped<IModularTenantEvents, CreateQueuesEvent>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
        }
    }
}
