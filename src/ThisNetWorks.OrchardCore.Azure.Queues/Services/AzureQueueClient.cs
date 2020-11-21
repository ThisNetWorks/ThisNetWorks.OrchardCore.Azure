using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace ThisNetWorks.OrchardCore.Azure.Queues.Services
{
    public class AzureQueueClient : IAzureQueueClient
    {
        private readonly QueueClient _queueClient;

        public AzureQueueClient(string connectionString, string name)
        {
            Name = name.ToLowerInvariant();

            _queueClient = new QueueClient(connectionString, Name);
        }

        public string Name { get; }

        public Task<Response<SendReceipt>> SendMessageAsync(string messageText)
            => _queueClient.SendMessageAsync(messageText);

        public Task<Response> CreateIfNotExistsAsync(IDictionary<string, string> metadata = default, CancellationToken cancellationToken = default)
            => _queueClient.CreateIfNotExistsAsync(metadata, cancellationToken);
    }
}

