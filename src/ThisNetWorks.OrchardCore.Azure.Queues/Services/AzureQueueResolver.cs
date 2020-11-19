using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ThisNetWorks.OrchardCore.Azure.Queues
{
    public class AzureQueueResolver : IAzureQueueResolver
    {
        private readonly AzureQueueOptions _azureQueueOptions;
        private readonly Dictionary<string, IAzureQueueClient> _azureQueueClients;

        public AzureQueueResolver(
            IOptions<AzureQueueOptions> azureQueueOptions, 
            IEnumerable<IAzureQueueClient> azureQueueClients)
        {
            _azureQueueOptions = azureQueueOptions.Value;
            _azureQueueClients = azureQueueClients.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
        }

        public IAzureQueueClient ResolveQueue(string queueName, Type type)
        {
            var queue = _azureQueueClients[queueName];
            if (queue == null)
            {
                throw new Exception($"Queue {queueName} not registered");
            }     

            var queueOption = _azureQueueOptions.Lookup[queueName];

            if (queueOption == null)
            {
                throw new Exception($"Queue {queueName} not registered");
            }

            if (queueOption.Type != type)
            {
                throw new Exception($"Incorrect type supplied for queue {queueName}. Expected {queueOption.Type}. Recieved {type.Name}");
            }

            return queue;
        }

    }
}