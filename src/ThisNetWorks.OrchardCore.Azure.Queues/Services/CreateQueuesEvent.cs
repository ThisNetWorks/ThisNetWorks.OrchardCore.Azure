using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Models;
using OrchardCore.Modules;

namespace ThisNetWorks.OrchardCore.Azure.Queues
{
    public class CreateQueuesEvent : ModularTenantEvents
    {
        private readonly AzureQueueOptions _options;
        private IEnumerable<IAzureQueueClient> _azureQueueClients;
        private readonly ShellSettings _shellSettings;
        private readonly ILogger _logger;

        public CreateQueuesEvent(
            IOptions<AzureQueueOptions> options,
            IEnumerable<IAzureQueueClient> azureQueueClients,
            ShellSettings shellSettings,
            ILogger<CreateQueuesEvent> logger
            )
        {
            _options = options.Value;
            _azureQueueClients = azureQueueClients;
            _shellSettings = shellSettings;
            _logger = logger;
        }

        public override async Task ActivatingAsync()
        {
            // Only create container if options are valid.

            if (_shellSettings.State != TenantState.Uninitialized &&
                !string.IsNullOrEmpty(_options.ConnectionString) &&
                _options.CreateQueues
                )
            {
                foreach(var queueClient in _azureQueueClients)
                {
                    _logger.LogDebug("Testing Azure Queue {QueueName} existence", queueClient.Name);

                    try
                    {
                        var result = await queueClient.CreateIfNotExistsAsync();
                        if (result == null)
                        {
                            _logger.LogDebug("Azure Queue {QueueName} already exists.", queueClient.Name);
                        }
                        else
                        {
                            _logger.LogDebug("Azure Queue {QueueName} created.", queueClient.Name);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Unable to create Azure Storage Queue {QueueName}.", queueClient.Name);
                    }
                }
            }
        }
    }
}
