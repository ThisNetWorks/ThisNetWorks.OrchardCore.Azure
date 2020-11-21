using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module
{
    public class AdminMenu : INavigationProvider
    {
        private readonly IStringLocalizer S;

        public AdminMenu(IStringLocalizer<AdminMenu> localizer)
        {
            S = localizer;
        }

        public Task BuildNavigationAsync(string name, NavigationBuilder builder)
        {
            if (!String.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
            {
                return Task.CompletedTask;
            }

            builder
                .Add(S["Azure Queue Samples"], queues => queues
                    .Action("Index", "Queue", new { area = "ThisNetWorks.OrchardCore.Azure.Queues.Sample.Module", action = "Index" })
                    .LocalNav()
                );

            return Task.CompletedTask;
        }
    }
}
