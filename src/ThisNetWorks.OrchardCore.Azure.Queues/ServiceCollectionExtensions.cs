using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using ThisNetWorks.OrchardCore.Azure.Queues;
using ThisNetWorks.OrchardCore.Azure.Queues.Services;

namespace Microsoft.Extensions.DependencyInjection
{    
    
    public static class AzureQueueServiceCollectionExtensions
    {
        public static IServiceCollection AddAnonymousAzureQueue(this IServiceCollection serviceCollection, string name)
            => serviceCollection.AddAzureQueue<JObject>(name, name);

        public static IServiceCollection AddAnonymousAzureQueue(this IServiceCollection serviceCollection, string name, string displayName)
            => serviceCollection.AddAzureQueue<JObject>(name, displayName);            

        public static IServiceCollection AddAzureQueue<T>(this IServiceCollection serviceCollection, string name, string displayName)
            => serviceCollection.AddAzureQueue(name, displayName, typeof(T));

        // this is the key most usefull one.
        public static IServiceCollection AddAzureQueue<T>(this IServiceCollection serviceCollection)
            => serviceCollection.AddAzureQueue(typeof(T).Name, typeof(T).Name, typeof(T));

        public static IServiceCollection AddAzureQueue<T>(this IServiceCollection serviceCollection, string displayName)
            => serviceCollection.AddAzureQueue(typeof(T).Name, displayName, typeof(T));            

        public static IServiceCollection AddAzureQueue(this IServiceCollection serviceCollection, string name, string displayName, Type type)
        {
            serviceCollection.Configure<AzureQueueOptions>(o => {
                o.AddAzureQueue(new AzureQueueOption { Name = name, DisplayName = displayName, Type = type });
            });

            serviceCollection.AddSingleton<IAzureQueueClient>(sp => {
                var options = sp.GetRequiredService<IOptions<AzureQueueOptions>>().Value;
                
                return new AzureQueueClient(options.ConnectionString, name);
            });

            return serviceCollection;
        }    
    }
}