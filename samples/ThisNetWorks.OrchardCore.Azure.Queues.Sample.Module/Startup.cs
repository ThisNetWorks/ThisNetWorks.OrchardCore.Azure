using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Admin;
using OrchardCore.Modules;
using OrchardCore.Mvc.Core.Utilities;
using OrchardCore.Navigation;
using ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module.Controllers;
using ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module.Models;

namespace ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module
{
    public class Startup : StartupBase
    {
        private readonly AdminOptions _adminOptions;

        public Startup(IOptions<AdminOptions> adminOptions)
        {
            _adminOptions = adminOptions.Value;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddAzureQueue<FooQueueIn>();

            services.AddScoped<INavigationProvider, AdminMenu>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            var queueControllerName = typeof(QueueController).ControllerName();
            routes.MapAreaControllerRoute(
                name: "QueueIndex",
                areaName: "ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module",
                pattern: _adminOptions.AdminUrlPrefix + "/Queue/Index",
                defaults: new { controller = queueControllerName, action = nameof(QueueController.Index) }
            );

            routes.MapAreaControllerRoute(
                name: "QueueSendMessage",
                areaName: "ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module",
                pattern: _adminOptions.AdminUrlPrefix + "/Queue/SendMessage",
                defaults: new { controller = queueControllerName, action = nameof(QueueController.SendMessage) }
            );            
        }
    }
}
